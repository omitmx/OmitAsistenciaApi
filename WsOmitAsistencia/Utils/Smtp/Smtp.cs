namespace WsOmitAsistencia.Utils.Smtp
{
    public class Smtp
    {
        public string Host { get; set; }
        public int Puerto { get; set; }
        public string Pwd { get; set; }
        public string CorreoRemitente { get; set; }
        public string CorreoBackups { get; set; }
    }
}
