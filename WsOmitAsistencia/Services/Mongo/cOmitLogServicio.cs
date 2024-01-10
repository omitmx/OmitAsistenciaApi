using MongoDB.Driver;
using WsOmitAsistencia.Models.OmitLog;

namespace WsOmitAsistencia.Services.Mongo
{
    public class cOmitLogServicio
    {
        private readonly string nombre_collection = "LogCitesoft";
        private IMongoCollection<vmOmitLog> olstLogs;
        public cOmitLogServicio(IMongoDB MgDbCnn)
        {
            var cnnMongo = new MongoClient(MgDbCnn.CadenaMongoDB);
            var CiteDb = cnnMongo.GetDatabase(MgDbCnn.BaseDatosLog);
            //dictamina la tabla/collection en mongo
            olstLogs = CiteDb.GetCollection<vmOmitLog>(nombre_collection);
        }
        public async Task<List<vmOmitLog>> GetAsync() =>
      await olstLogs.Find(_ => true).ToListAsync();

        public async Task CreateAsync(vmOmitLog nuevoLog) =>
       await olstLogs.InsertOneAsync(nuevoLog);
        public vmOmitLog Add(vmOmitLog nuevoLog)
        {
            try
            {
                olstLogs.InsertOne(nuevoLog);

            }
            catch
            {
            }
            return nuevoLog;
        }
        public vmOmitLog Get(string key)
        {
            var oEncontrado = new vmOmitLog();
            try
            {
                oEncontrado = olstLogs.Find(x => x.error_key == key).FirstOrDefault();

            }
            catch
            {
            }
            return oEncontrado;
        }
    }
}
