using WsOmitAsistencia.Models;

namespace WsOmitAsistencia.Services
{
    public interface ICrudGeneral
    {
        public vmRespuesta GetLst(int id);
       public  vmRespuesta Add<TModel>(TModel model);
        public vmRespuesta Edit<TModel>(TModel model);
        public vmRespuesta Delete(int id);
    }
}
