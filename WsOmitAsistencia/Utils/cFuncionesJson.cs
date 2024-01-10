using System.Text.Json;

namespace WsOmitAsistencia.Utils
{
    public class cFuncionesJson
    {

        public static string SerializeJson(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }
        public static TModel DeserializeJson<TModel>(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                return default(TModel);
            }

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                // Agrega otras opciones de serialización aquí según sea necesario
            };

            return JsonSerializer.Deserialize<TModel>(json, options);
        }
    }
}
