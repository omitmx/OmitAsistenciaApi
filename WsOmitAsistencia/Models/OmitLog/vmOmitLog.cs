using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace WsOmitAsistencia.Models.OmitLog
{
    public class vmOmitLog
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        [BsonElement("id")]
        public string id { get; set; }
        [BsonElement("error_key")]
        public string error_key { get; set; }
        [BsonElement("codigo_error")]
        public int CodigoError { get; set; }
        [BsonElement("contexto")]
        public string contexto { get; set; }
        [BsonElement("mensaje")]
        public string Mensaje { get; set; }
        [BsonElement("data")]
        public string data { get; set; }
        [BsonElement("login")]
        public string login { get; set; }
        [BsonRepresentation(BsonType.DateTime)]
        [BsonElement("fecha")]
        public DateTime fecha { get; set; }

    }
}
