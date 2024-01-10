using WsOmitAsistencia.Services;

namespace WsOmitAsistencia.Utils.Db
{
    public class MongoDBConn : IMongoDB
    {
        public string CadenaMongoDB { get; set; }
        public string BaseDatosLog { get; set; }
    }
}
