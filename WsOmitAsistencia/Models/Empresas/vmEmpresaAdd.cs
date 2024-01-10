namespace WsOmitAsistencia.Models.Empresas
{
    public class vmEmpresaAdd
    {

        public int id { get; set; }
        public string empresa { get; set; }
        public string razon_social { get; set; }
        public string direccion_fiscal { get; set; }
        public string rfc { get; set; }
        public int cp { get; set; }
        public string ciudad { get; set; }
        public string logo { get; set; }
        public string url_logo { get; set; }
        public int baja { get; set; }
    }
}
