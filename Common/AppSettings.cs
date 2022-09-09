﻿using Microsoft.Extensions.Configuration;

namespace Common
{
    public class AppSettings
    {
        private AppSettings() { }

        private static AppSettings _instance;

        public static AppSettings GetInstance()
        {
            if (_instance == null)
            {
                _instance = new AppSettings();
            }
            return _instance;
        }

        [ConfigurationKeyName("Rut")] 
        public string Rut { get; set; }

        [ConfigurationKeyName("RutReceptorDefault")]
        public string RutDefault { get; set; }

        [ConfigurationKeyName("NombreEmpresa")]
        public string NombreEmpresa { get; set; }

        [ConfigurationKeyName("TokenExpired")]
        public int TokenExpired { get; set; }

        private int _Intervalo;
        [ConfigurationKeyName("Intervalo")]
        public int Intervalo
        {
            get
            {
                return this._Intervalo;
            }
            set
            {
                if (this._Intervalo == 0)
                    this._Intervalo = 1;
                int milisegundos = this._Intervalo * 3600000;//60*60*1000;
                this._Intervalo = milisegundos;
            }
        }

        [ConfigurationKeyName("Servicios")]
        public EndPoints EndPoint { get; set; }

        [ConfigurationKeyName("Certificado")]
        public Certificado Certificado { get; set; }


        [ConfigurationKeyName("MaxDateEstado")]
        public int MaxDate { get; set; }
    }
}