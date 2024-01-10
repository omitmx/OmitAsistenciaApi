using FirebirdSql.Data.FirebirdClient;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WsOmitAsistencia.Models.Sesiones;
using WsOmitAsistencia.Models;
using WsOmitAsistencia.Services.Mongo;
using WsOmitAsistencia.Utils.Jwt;
using WsOmitAsistencia.Utils.Db;
using WsOmitAsistencia.Utils;
using Dapper;

namespace WsOmitAsistencia.Services
{
    public class cUsuServicio : IUsuServicio
    {
        private readonly AppSettings _appSettings;
        private readonly FBCnn _fbCnn;
        private readonly cOmitLogServicio _logError;//servicio para guardar errores en mongoDB
        public cUsuServicio(IOptions<AppSettings> appSettings, IOptions<FBCnn> fbCnn, cOmitLogServicio logError)
        {
            _appSettings = appSettings.Value;
            _fbCnn = fbCnn.Value;
            _logError = logError;
        }


        public cUsuarioRespuesta Autentificar(vmAccess model)
        {
            cUsuarioRespuesta oUsuRes = new cUsuarioRespuesta();
            oUsuRes.token = "";
            try
            {
                string pwdEncryp = cFuncionesPublicas.GetSha256(model.pwd);
                string qUsu = "select usuario_key id,(nombres ||' '|| app ||' '|| apm) nombre_completo, nombres, app, apm, login,  correo, tipo_usuario_link tipo_usuario_id from cat_usuarios  where login=@login and pwd=@pwd and baja =0";

                using (var conDbFB = new FbConnection(_fbCnn.CnnAsisFB))
                {

                    var oUsu = conDbFB.Query<vmSesionUsuario>(qUsu, new { login = model.login.ToUpper(), pwd = pwdEncryp }).FirstOrDefault();
                    if (oUsu != null)
                    {
                        if (oUsu.id > 0)
                        {
                            oUsuRes.token = GetToken(model, oUsu);
                            oUsuRes.info_usuario = oUsu;
                        }
                    }


                }


            }
            catch (Exception ex)
            {
                #region LOG_ERROR
                //guardar la excepcion
                string UserId = cVarPublicas.USU_SYS_OMIT;
                var oError = cFuncionesPublicas.CrearLogOmit(UserId, ex);

                _logError.Add(oError);
                #endregion
            }
            return oUsuRes;
        }



        private string GetToken(vmAccess model, vmSesionUsuario info)
        {
            var tokenHdler = new JwtSecurityTokenHandler();

            var llave = Encoding.ASCII.GetBytes(_appSettings.Secreto);
            var tokenDesc = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(
                    new Claim[] {
                    new Claim(ClaimTypes.NameIdentifier,model.login),
                    new Claim(ClaimTypes.Name,model.login),
                     new Claim(ClaimTypes.UserData,cFuncionesJson.SerializeJson(info))
                    }),
                Expires = DateTime.UtcNow.AddDays(10),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(llave), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHdler.CreateToken(tokenDesc);
            return tokenHdler.WriteToken(token);
        }


    }
}
