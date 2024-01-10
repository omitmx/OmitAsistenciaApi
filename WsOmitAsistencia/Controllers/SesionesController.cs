using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WsOmitAsistencia.Models;
using WsOmitAsistencia.Services.Mongo;
using WsOmitAsistencia.Utils;

namespace WsOmitAsistencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [SwaggerTag("Web API para autenticación de Usuarios OMIT.", cVarPublicas.DIR_URL_DOCUMENTACION_PDF)]

    public class SesionesController : ControllerBase
    {
        private IUsuServicio _usuServicio;
        private readonly cOmitLogServicio _logError;//servicio para guardar errores en mongoDB
        /// <summary>
        /// 
        /// </summary>
        /// <param name="usuServicio"></param>
        /// <param name="logError"></param>
        public SesionesController(IUsuServicio usuServicio, cOmitLogServicio logError)
        {
            _usuServicio = usuServicio;
            _logError = logError;
        }



        /// <summary>
        /// Inicio de sesion de usuario.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [SwaggerOperation("Inicio de sesion de usuario")]
        [SwaggerResponse(200, "Solicitud exitosa")]
        //[ProducesResponseType(StatusCodes.Status401Unauthorized)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult login(vmAccess model)
        {
            vmRespuesta oRespuesta = new vmRespuesta();
            oRespuesta.resultado = 0;
            oRespuesta.msg = "";
            try
            {
                if (ModelState.IsValid)
                {
                    cUsuarioRespuesta oRes = _usuServicio.Autentificar(model);
                    if (oRes == null)
                    {
                        oRespuesta.resultado = 0;
                        oRespuesta.msg = "Usuario o contraseña incorrecta";
                        return BadRequest(oRespuesta);
                    }
                    else
                    {
                        oRespuesta.resultado = 1;
                        if (oRes.token == "")
                        {
                            oRespuesta.resultado = 0;
                            oRespuesta.msg = "Token no generado...!";
                        }
                        oRespuesta.data = cFuncionesJson.SerializeJson(oRes);
                    }
                }
                else
                {
                    string messages = string.Join("; ", ModelState.Values
                                                           .SelectMany(x => x.Errors)
                                                           .Select(x => x.ErrorMessage));
                    oRespuesta.data = null;
                    oRespuesta.msg = messages;
                }
            }
            catch (Exception ex)
            {
                #region LOG_ERROR
                //guardar la excepcion
                string UserId = User.Identity.Name.ToString();
                var oError = cFuncionesPublicas.CrearLogOmit(UserId, ex);
                var info_app = User.Claims.Where(d => d.Type == ClaimTypes.UserData).FirstOrDefault();

                _logError.Add(oError);
                oRespuesta.msg = $"Código:{oError.error_key}, {Environment.NewLine}Favor de intentar mas tarde...!";
                #endregion
            }

            return Ok(oRespuesta);
        }
    }

}
