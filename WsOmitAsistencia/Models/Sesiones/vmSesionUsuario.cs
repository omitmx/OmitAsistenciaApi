namespace WsOmitAsistencia.Models.Sesiones
{
    public class vmSesionUsuario
    {
        public long id { get; set; }
        public string nombre_completo { get; set; }
        public string nombres { get; set; }
        public string app { get; set; }
        public string apm { get; set; }
        public string login { get; set; }
        public string correo { get; set; }
        public int tipo_usuario_id { get; set; }
    }
}
