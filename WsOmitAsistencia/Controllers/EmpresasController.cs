using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using WsOmitAsistencia.Models;
using WsOmitAsistencia.Models.Empresas;
using WsOmitAsistencia.Services.Mongo;
using WsOmitAsistencia.Utils;
using WsOmitAsistencia.Utils.Db;
using WsOmitAsistencia.Utils.Empresas;

namespace WsOmitAsistencia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    [SwaggerTag("Web API para autenticación de Usuarios OMIT.", cVarPublicas.DIR_URL_DOCUMENTACION_PDF)]
    public class EmpresasController : ControllerBase
    {
        private readonly cEmpresa oEmpresa;
        private readonly FBCnn _fbCnn;
        private readonly cOmitLogServicio _logError;//servicio para guardar errores en mongoDB
        public EmpresasController(IOptions<FBCnn> fbCnn, cOmitLogServicio logError)
        {
            _fbCnn = fbCnn.Value;
            _logError = logError;
            oEmpresa = new cEmpresa(_fbCnn.CnnAsisFB);
        }
        [HttpGet("GetEmpresas")]
        
        public vmRespuesta GetEmpresas(int id)
        {
            vmRespuesta oRespuesta = new vmRespuesta();
            try
            {
                oRespuesta = oEmpresa.GetLst(id);
            }
            catch (Exception ex)
            {
                #region LOG_ERROR
                //guardar la excepcion
                string UserId = "Omit";
                if (User.Identity != null)
                {
                    UserId = User.Identity.Name ?? "Omit";
                    var info_app = User.Claims.Where(d => d.Type == ClaimTypes.UserData).FirstOrDefault();

                }
                var oError = cFuncionesPublicas.CrearLogOmit(UserId, ex);

                _logError.Add(oError);
                oRespuesta.msg = $"Código:{oError.error_key}, {Environment.NewLine}Favor de intentar mas tarde...!";
                #endregion
            }

            return oRespuesta;
        }
        [HttpPost("SetEmpresa")]
        public vmRespuesta SetEmpresa(vmEmpresaAdd model)
        {
            vmRespuesta oRespuesta = new vmRespuesta();
            try
            {
                oRespuesta = oEmpresa.Add(model);
            }
            catch (Exception ex)
            {
                #region LOG_ERROR
                //guardar la excepcion
                string UserId = "Omit";
                if (User.Identity != null)
                {
                    UserId = User.Identity.Name ?? "Omit";
                    var info_app = User.Claims.Where(d => d.Type == ClaimTypes.UserData).FirstOrDefault();

                }
                var oError = cFuncionesPublicas.CrearLogOmit(UserId, ex);

                _logError.Add(oError);
                oRespuesta.msg = $"Código:{oError.error_key}, {Environment.NewLine}Favor de intentar mas tarde...!";
                #endregion
            }

            return oRespuesta;
        }
    }
}
