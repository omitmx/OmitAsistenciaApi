namespace WsOmitAsistencia.Models
{
    public class vmRespuesta
    {
        public int resultado { get; set; }
        public object data { get; set; }
        public string msg { get; set; }
        public vmRespuesta()
        {
            resultado = 0;
            data = null;
            msg = "";
        }
    }
}
