using Common;
using Common.BL;
using Common.SOAP.Response;
using DbCore.Service;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Globalization;
using WorkerService.Service;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private const string _codigo = "codigo";
        private const string _descripcion = "descripcion";
        private Serilog.Core.Logger _log = Logger.GetInstance()._Logger;
        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    //   Logger.GetInstance().Dispose();
                    //   _log = null;
                    //   _log = Logger.GetInstance()._Logger;
                    _log.Information(string.Format("Iniciando Proceso {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    _logger.LogInformation(string.Format("Iniciando Proceso {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    string pathJson = Path.Combine(AppContext.BaseDirectory, "appsettings.json");
                    _log.Information(pathJson);
                    IConfigurationRoot config = new ConfigurationBuilder().AddJsonFile(pathJson, true, true).Build();
                    AppSettings appSettings = AppSettings.GetInstance();
                    config.Bind("AppSettings", appSettings);

                    DocumentoService contextDB = new DocumentoService();
                    EstadoService _contextEstadoDB = new EstadoService();

                    #region "FIND DOCUMENTS"
                    try
                    {
                        ResponseDocuments rq = await WSDocuments.FindDocumentos();
                        if (rq.PDocumentos.Count > 0)
                        {
                            foreach (var document in rq.PDocumentos)
                            {
                                try
                                {
                                    DateTime fechaEmision = DateTime.ParseExact(document.FechaEmision.ToString(), "yyyyMMdd", CultureInfo.InvariantCulture);
                                    contextDB.Add(document.RutCliente, document.TipoDocumentoCobro, fechaEmision, document.NumeroDocumentoCobro, document.SaldoImpago, document.ValorIva, document.ValorNeto);
                                }
                                catch (DbUpdateException ex)
                                {
                                    _log.Error(string.Format("error proceso save documents {0}-stack{1}", ex.Message, ex.StackTrace));
                                    _logger.LogError(string.Format("error proceso save documents {0}-stack{1}", ex.Message, ex.StackTrace));
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        _log.Error(string.Format("error proceso findocuments save documents {0}-stack{1}", ex.Message, ex.StackTrace));
                        _logger.LogError(string.Format("error proceso findocuments save documents {0}-stack{1}", ex.Message, ex.StackTrace));
                    }
                    #endregion
                    #region "SII"
                    try
                    {
                        WSFacturacionElectronica wsFactura = new WSFacturacionElectronica();
                        Token auth = await wsFactura.GetToken(appSettings.TokenExpired);
                        List<Documentos> documents = contextDB.GetDocuments();
                        foreach (var document in documents)
                        {
                            // RENUEVA TOKEN SI EXPIRO
                            if (!auth.ValidateToken(auth))
                            { auth = await wsFactura.GetToken(appSettings.TokenExpired); }

                            int rut = int.Parse(document.RutReceptor.Split('-')[0]);
                            string estado = await wsFactura.GetEstado(auth.token, document.RutEmisor, document.NumeroDocumento,
                                document.TipoDocumento, rut, document.RutReceptor.Split('-')[1], document.Total, document.FechaEmision);

                            string codigo = string.Empty;
                            string descripcion = string.Empty;
                            try
                            {
                                JObject response = JObject.Parse(estado);
                                if (response.ContainsKey(_codigo))
                                {
                                    codigo = response.GetValue(_codigo).ToString();
                                }
                                if (response.ContainsKey(_descripcion))
                                {
                                    descripcion = response.GetValue(_descripcion).ToString();
                                }
                                _contextEstadoDB.Add(document, codigo, descripcion);
                                contextDB.Update(document.Id, descripcion);
                            }
                            catch (DbUpdateException ex)
                            {
                                _log.Error(string.Format("error save estado documents {0}-stack{1}", ex.Message, ex.StackTrace));
                                _logger.LogError(string.Format("error save estado documents {0}-stack{1}", ex.Message, ex.StackTrace));
                            }
                            catch (JsonReaderException ex)
                            {
                                _log.Error(string.Format("JsonReaderException document id: {0} error: {1}", document.Id, ex.Message));
                                codigo = ex.Message;
                                descripcion = ex.Message;
                            }
                        }
                        wsFactura = null;
                    }
                    catch (Exception ex)
                    {
                        _log.Error(string.Format("error proceso findocuments save documents {0}-stack{1}", ex.Message, ex.StackTrace));
                        _logger.LogError(string.Format("error proceso findocuments save documents {0}-stack{1}", ex.Message, ex.StackTrace));
                    }
                    #endregion
                    #region "ESTATUS"
                    List<Documentos> documentsEnvio = contextDB.GetDocumentsWithStatus();
                    foreach (var document in documentsEnvio)
                    {
                        if (document.LineasEstado.Count == 0)
                        {
                            continue;
                        }
                        if (document.Estado1 == document.Estado2)
                        { //ya no puedo controlar eso 
                            continue;
                        }

                        try
                        {
                            JObject response = JObject.Parse(await WSEstadoDocumento.Invoke(JsonConvert.SerializeObject(document)));
                            if (!response.ContainsKey("entrega"))
                            {
                                _log.Error(string.Format("error respuesta no definida GetDocumentsWithStatus document {0} {1}", document.Id, JsonConvert.SerializeObject(response)));
                                continue;
                            }
                            string ok = response["entrega"][0]["retorno"].ToString();
                            if (ok.ToLower() == "error")
                            {
                                _log.Error(string.Format("error respuesta GetDocumentsWithStatus document {0} {1}", document.Id, JsonConvert.SerializeObject(response)));
                            }
                            else
                            {
                                contextDB.Update(document.Id);
                            }

                        }
                        catch (Exception ex)
                        {
                            _log.Error(string.Format("error proceso GetDocumentsWithStatus document {0} {1}-stack{2}", document.Id, ex.Message, ex.StackTrace));
                            _logger.LogError(string.Format("error proceso GetDocumentsWithStatus document {0} {1}-stack{2}", document.Id, ex.Message, ex.StackTrace));
                        }

                    }
                    #endregion

                    _log.Information(string.Format("Finalizando Proceso {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                    _logger.LogInformation(string.Format("Finalizando Proceso {0}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")));
                }
                catch (Exception ex)
                {
                    _log.Error(string.Format("error {0}-stack{1}", ex.Message, ex.StackTrace));
                    _logger.LogError(ex.Message);
                }
                finally
                {
                    GC.Collect();
                }

                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(AppSettings.GetInstance().Intervalo, stoppingToken);
            }
        }
    }
}