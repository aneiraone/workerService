using Serilog;

public class Logger : IDisposable
{
    public Serilog.Core.Logger _Logger;
    // public Serilog.Core.Logger _LoggerFile;
    //public Serilog.Core.Logger _LoggerData;

    private Logger()
    {
        string location = System.Reflection.Assembly.GetEntryAssembly().Location;

        string path = Path.Combine(Path.GetDirectoryName(location), "log",
            string.Format("{0}.log", DateTime.Now.ToString("yyyy_MM_dd")));

        //string pathDocuments = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location), "log",
        //    string.Format("{0}_documents.log", DateTime.Now.ToString("yyyy_MM_dd")));

        _Logger = new LoggerConfiguration().WriteTo.File(path,
            rollingInterval: RollingInterval.Day
            ).CreateLogger();
        //  _LoggerFile = new LoggerConfiguration().WriteTo.File(pathDocuments).CreateLogger();
        // _LoggerData = new LoggerConfiguration().WriteTo.File(pathData).CreateLogger();
    }

 
    private static Logger _instance;

    public static Logger GetInstance()
    {
        //if (reset){_instance = new Logger();}
        if (_instance == null)
        {
            _instance = new Logger();
        }

        return _instance;
    }

    public void Dispose()
    {
        //Release collection
      //  _instance.Clear();
        _instance = null;
        _Logger = null;
    }


}