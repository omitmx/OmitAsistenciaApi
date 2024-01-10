namespace WsOmitAsistencia.Models
{
    public interface IUsuServicio
    {
        cUsuarioRespuesta Autentificar(vmAccess model);
    }
}
