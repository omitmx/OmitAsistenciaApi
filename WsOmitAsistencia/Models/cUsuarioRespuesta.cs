using WsOmitAsistencia.Models.Sesiones;

namespace WsOmitAsistencia.Models
{
    public class cUsuarioRespuesta
    {
        public string token { get; set; }
        public vmSesionUsuario info_usuario { get; set; }
        public cUsuarioRespuesta()
        {
            info_usuario = new vmSesionUsuario();
        }
    }
}
