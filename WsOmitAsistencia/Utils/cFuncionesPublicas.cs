using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.Text.Json;
using System.Text;
using System.Xml;
using WsOmitAsistencia.Models.OmitLog;
using System.Security.Cryptography;

namespace WsOmitAsistencia.Utils
{
    public enum MethodoHttp
    {
        GET = 0,
        POST = 1,
        PUT = 2,
        DELETE = 3,
        PROPFIND = 4,
        MKCOL = 5
    }
    public enum TipoData
    {
        JSON = 0,
        XML = 1,
        NONE = 2
    }
    public class cFuncionesPublicas
    {
        public static bool DsTieneDatos(DataSet ds)
        {
            bool ok = false;
            if (ds != null)
            {
                if (ds.Tables.Count > 0)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        ok = true;
                    }
                }
            }

            return ok;
        }
        public static bool DtTieneDatos(DataTable dt)
        {
            bool ok = false;
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    ok = true;
                }
            }

            return ok;
        }
        public static int CInt2(object valor)
        {
            int IntConvertido = 0;
            try
            {
                IntConvertido = Convert.ToInt32(valor);
            }
            catch// (Exception ex)
            {
                IntConvertido = 0;
            }
            return IntConvertido;
        }
        public static double CDbl2(object valor)
        {
            double DblConvertido = 0;
            try
            {
                DblConvertido = Convert.ToDouble(valor);
            }
            catch //(Exception ex)
            {
                DblConvertido = 0;
            }
            return DblConvertido;
        }
        public static decimal CDec2(object valor)
        {
            decimal DblConvertido = 0;
            try
            {
                DblConvertido = Convert.ToDecimal(valor);
            }
            catch //(Exception ex)
            {
                DblConvertido = 0;
            }
            return DblConvertido;
        }

        public static bool CBool2(object valor)
        {
            bool DblConvertido = false;
            try
            {
                DblConvertido = Convert.ToBoolean(valor);
            }
            catch// (Exception ex)
            {
                DblConvertido = false;
            }
            return DblConvertido;
        }
        public static XmlElement SerializeXml(object obj)
        {
            XmlElement serializedXmlElement = null;

            try
            {
                System.IO.MemoryStream memoryStream = new MemoryStream();
                System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(obj.GetType());
                xmlSerializer.Serialize(memoryStream, obj);
                memoryStream.Position = 0;

                XmlDocument xmlDocument = new XmlDocument();
                xmlDocument.Load(memoryStream);
                serializedXmlElement = xmlDocument.DocumentElement;
                memoryStream.Dispose();
            }
            catch// (Exception e)
            {
                //logging statements. You must log exception for review
            }

            return serializedXmlElement;
        }
        public static string SerializeJson2(object obj)
        {
            return JsonSerializer.Serialize(obj);
        }
        // usuarios Seguridad Encriptar
        public static string GetSha256(string str)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = Encoding.UTF8.GetBytes(str);
                byte[] hashBytes = sha256.ComputeHash(bytes);
                StringBuilder sb = new StringBuilder();
                foreach (byte b in hashBytes)
                {
                    sb.Append(b.ToString("x2"));
                }
                return sb.ToString();
            }
        }

        public static byte[] ConvertirBase64ToByte(string base64String)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64String);
            return base64EncodedBytes;
        }

        public static string ConvertirByteToBase64(byte[] data)
        {
            var base64EncodedBytes = System.Convert.ToBase64String(data);
            return base64EncodedBytes;
        }



        public static string GetValorMetodo(MethodoHttp metodo)
        {
            string resultado = "";
            if (metodo == MethodoHttp.GET)
                resultado = "GET";
            else if (metodo == MethodoHttp.POST)
                resultado = "POST";
            else if (metodo == MethodoHttp.PUT)
                resultado = "PUT";
            else if (metodo == MethodoHttp.DELETE)
                resultado = "DELETE";
            else if (metodo == MethodoHttp.PROPFIND)
                resultado = "PROPFIND";
            else if (metodo == MethodoHttp.MKCOL)
                resultado = "MKCOL";


            return resultado;
        }


        public static string htmlCreacion(string GuidIdImg, string Msg_Hteml)
        {
            string html = "";
            string temp = "<!DOCTYPE html>" +
            "<html>" +
            "<head>" +
                "<meta charset='utf-8' />" +
                "<title>Notificaciones Citelum</title>" +
            "</head>" +
            "<body>" +
                "<p><h1 style='color:darkblue'>CITELUM MEXICO S.A DE C.V</h1></p>" +
                $"<p>{Msg_Hteml}</p><br />" +
                $"<img src='cid:{GuidIdImg}'/><br /><br>" +//;"<img src='Images/Corpo/CTLMXCorreo.png' /><br /><br>" +
                "<p style='color:gray;font-size:x-small;text-align:justify;'>" +
                     "Este mensaje y los archivos adjuntos son de la exclusiva responsabilidad del remitente y están destinados únicamente a los destinatarios. Asi mismo, son de carácter confidencial." +
                    "Se prohíbe cualquier divulgación, revelación o uso no autorizado. Si no es el destinatario de este mensaje, elimínelo y notifíquelo al remitente." +
                    "La integridad de este mensaje no se puede garantizar en internet, por lo tanto, Citelum no será responsable del mensaje si es modificado." +
                "</p><br />" +
            "</body>" +
            "</html>";



            html = temp;
            return html;
        }
        public static string htmlCreacionDosImagenes(string GuidIdImg, string Msg_Hteml, string B64Img)
        {
            string html = "";


            string temp = "<!DOCTYPE html>" +
            "<html>" +
            "<head>" +
                "<meta charset=\"utf-8\" />" +
                "<title>Notificaciones Citelum</title>" +
            "</head>" +
            "<body>" +
                "<p><h1 style=\"color:darkblue\">CITELUM MEXICO S.A DE C.V</h1></p><br>" +
                 $"<img src=\"cid:{GuidIdImg}\" style=\"width: 216px; height: 56px\" /><br>" +
                $"<p>{Msg_Hteml}</p><br>" +
                //$"<img src=\"cid:{GuidIdImg2}\" alt={GuidIdImg2}  /><br>" +
                $"<img src=\"data:image/jpg;base64,{B64Img}\" " +
                "<p style=\"color:gray;font-size:x-small;text-align:justify;\">" +
                     "Este mensaje y los archivos adjuntos son de la exclusiva responsabilidad del remitente y están destinados únicamente a los destinatarios. Asi mismo, son de carácter confidencial. " +
                    "Se prohíbe cualquier divulgación, revelación o uso no autorizado. Si no es el destinatario de este mensaje, elimínelo y notifíquelo al remitente. " +
                    "La integridad de este mensaje no se puede garantizar en internet, por lo tanto, Citelum no será responsable del mensaje si es modificado." +
                "</p><br />" +
            "</body>" +
            "</html>";


            ////sin cuerpo html5
            //temp = "<p><h1 style=\"color:darkblue\">CITELUM MEXICO S.A DE C.V</h1></p><br>" +
            //     $"<img src=\"{GuidIdImg}\" style=\"width: 216px; height: 56px\" /><br>" +
            //    $"<p>{Msg_Hteml}</p><br>" +
            //     $"<img src=\"{GuidIdImg2}\" alt={GuidIdImg2}  /><br>" +
            //    "<p style=\"color:gray;font-size:x-small;text-align:justify;\">" +
            //         "Este mensaje y los archivos adjuntos son de la exclusiva responsabilidad del remitente y están destinados únicamente a los destinatarios. Asi mismo, son de carácter confidencial." +
            //        "Se prohíbe cualquier divulgación, revelación o uso no autorizado. Si no es el destinatario de este mensaje, elimínelo y notifíquelo al remitente." +
            //        "La integridad de este mensaje no se puede garantizar en internet, por lo tanto, Citelum no será responsable del mensaje si es modificado." +
            //    "</p><br />";
            html = temp;
            return html;
        }
        public static string htmlCreacionDosImagenesRutaFisica(string GuidIdImg, string Msg_Hteml, string GuidIdImg2)
        {
            string html = "";


            string temp = "<!DOCTYPE html>" +
            "<html>" +
            "<head>" +
                "<meta charset=\"utf-8\" />" +
                "<title>Notificaciones Citelum</title>" +
            "</head>" +
            "<body>" +
                "<p><h1 style=\"color:darkblue\">CITELUM MEXICO S.A DE C.V</h1></p><br>" +
                 $"<img src=\"cid:{GuidIdImg}\" style=\"width: 216px; height: 56px\" /><br>" +
                $"<p>{Msg_Hteml}</p><br>" +
                $"<img src=\"cid:{GuidIdImg2}\" style=\"width: 200px; height: 200px\"  /><br>" +
                "<p style=\"color:gray;font-size:x-small;text-align:justify;\">" +
                     "Este mensaje y los archivos adjuntos son de la exclusiva responsabilidad del remitente y están destinados únicamente a los destinatarios. Asi mismo, son de carácter confidencial. " +
                    "Se prohíbe cualquier divulgación, revelación o uso no autorizado. Si no es el destinatario de este mensaje, elimínelo y notifíquelo al remitente. " +
                    "La integridad de este mensaje no se puede garantizar en internet, por lo tanto, Citelum no será responsable del mensaje si es modificado." +
                "</p><br />" +
            "</body>" +
            "</html>";


            ////sin cuerpo html5
            //temp = "<p><h1 style=\"color:darkblue\">CITELUM MEXICO S.A DE C.V</h1></p><br>" +
            //     $"<img src=\"{GuidIdImg}\" style=\"width: 216px; height: 56px\" /><br>" +
            //    $"<p>{Msg_Hteml}</p><br>" +
            //     $"<img src=\"{GuidIdImg2}\" alt={GuidIdImg2}  /><br>" +
            //    "<p style=\"color:gray;font-size:x-small;text-align:justify;\">" +
            //         "Este mensaje y los archivos adjuntos son de la exclusiva responsabilidad del remitente y están destinados únicamente a los destinatarios. Asi mismo, son de carácter confidencial." +
            //        "Se prohíbe cualquier divulgación, revelación o uso no autorizado. Si no es el destinatario de este mensaje, elimínelo y notifíquelo al remitente." +
            //        "La integridad de este mensaje no se puede garantizar en internet, por lo tanto, Citelum no será responsable del mensaje si es modificado." +
            //    "</p><br />";
            html = temp;
            return html;
        }
        public static string htmlCreacion(string GuidIdImg, string Msg_Hteml, string EmpresaDB)
        {
            string html = "";
            string temp = "<!DOCTYPE html>" +
            "<html>" +
            "<head>" +
                "<meta charset='utf-8' />" +
                "<title>Notificaciones Citelum</title>" +
            "</head>" +
            "<body>" +
                $"<p><h1 style='color:darkblue'>{EmpresaDB}</h1></p>" +
                $"<p>{Msg_Hteml}</p><br />" +
                $"<img src='cid:{GuidIdImg}'/><br /><br>" +//;"<img src='Images/Corpo/CTLMXCorreo.png' /><br /><br>" +
                "<p style='color:gray;font-size:x-small;text-align:justify;'>" +
                     "Este mensaje y los archivos adjuntos son de la exclusiva responsabilidad del remitente y están destinados únicamente a los destinatarios. Asi mismo, son de carácter confidencial." +
                    "Se prohíbe cualquier divulgación, revelación o uso no autorizado. Si no es el destinatario de este mensaje, elimínelo y notifíquelo al remitente." +
                    "La integridad de este mensaje no se puede garantizar en internet, por lo tanto, Citelum no será responsable del mensaje si es modificado." +
                "</p><br />" +
            "</body>" +
            "</html>";



            html = temp;
            return html;
        }
        public static string htmlBodyChecadas(string GuidIdImg, string Msg_Html)
        {
            string html = "";
            string temp = "<!DOCTYPE html>" +
            "<html>" +
            "<head>" +
                "<meta charset='utf-8' />" +
                "<title>Asistencia de empelados</title>" +
            "</head>" +
            "<body>" +
                $"<p><h1 style='color:darkblue'>CITELUM MEXICO S.A DE C.V</h1></p>" +
                $"<p>{Msg_Html}</p><br />" +
                $"<img src='cid:{GuidIdImg}'/><br /><br>" +//;"<img src='Images/Corpo/CTLMXCorreo.png' /><br /><br>" +
                "<p style='color:gray;font-size:x-small;text-align:justify;'>" +
                     "Este mensaje y los archivos adjuntos son de la exclusiva responsabilidad del remitente y están destinados únicamente a los destinatarios. Asi mismo, son de carácter confidencial." +
                    "Se prohíbe cualquier divulgación, revelación o uso no autorizado. Si no es el destinatario de este mensaje, elimínelo y notifíquelo al remitente." +
                    "La integridad de este mensaje no se puede garantizar en internet, por lo tanto, Citelum no será responsable del mensaje si es modificado." +
                "</p><br />" +
            "</body>" +
            "</html>";



            html = temp;
            return html;
        }

        public static vmOmitLog CrearLogOmit(string Usuario, Exception error)
        {

            var e_key = DateTime.Now.ToString("yyyyMMddHHmmssFFF");
            var oError = new vmOmitLog();
            oError.login = Usuario;
            oError.CodigoError = error.HResult;
            oError.error_key = e_key;
            oError.Mensaje = error.Message;
            oError.fecha = DateTime.UtcNow;
            oError.contexto = error.Source;
            oError.data = error.StackTrace;
            return oError;
        }
        public static vmOmitLog CrearLogOmit(string Usuario, FbException error)
        {

            var e_key = DateTime.Now.ToString("yyyyMMddHHmmssFFF");
            var oError = new vmOmitLog();
            oError.login = Usuario;
            oError.CodigoError = error.HResult;
            oError.error_key = e_key;
            oError.Mensaje = error.Message;
            oError.fecha = DateTime.UtcNow;
            oError.contexto = error.Source;
            oError.data = error.StackTrace;
            return oError;
        }


    }
}
