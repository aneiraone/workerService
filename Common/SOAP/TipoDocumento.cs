namespace Common.SOAP
{
    public static class TipoDocumento
    {
        public static string Get(string tipo)
        {
            switch (tipo)
            {
                case "1": return "39";
                case "2": return "2";
                default: return "0";
            }
        }
    }
}
