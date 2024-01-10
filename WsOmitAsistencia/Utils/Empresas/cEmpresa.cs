using Dapper;
using FirebirdSql.Data.FirebirdClient;
using WsOmitAsistencia.Models;
using WsOmitAsistencia.Models.Empresas;
using WsOmitAsistencia.Services;

namespace WsOmitAsistencia.Utils.Empresas
{
    public class cEmpresa
    {
        public string CadenaFB { get; set; }
        public cEmpresa(string context)
        {
            CadenaFB = context;
        }


        public vmRespuesta GetLst(int id)
        {
            vmRespuesta oRespuesta = new vmRespuesta();
            try
            {
                string qUsu = "select empresa_key id, empresa, razon_social, direccion_fiscal, rfc, cp, ciudad, logo, url_logo, baja from get_empresas(@id)";
                using (var conFB = new FbConnection(CadenaFB))
                {
                    var olst = conFB.Query<vmEmpresaInfo>(qUsu, new { id = id });
                    if (olst != null)
                    {
                        List<vmEmpresaInfo> lstUsu = olst.ToList();
                        if (lstUsu.Count > 0)
                        {
                            oRespuesta.resultado = 1;
                            oRespuesta.data = lstUsu;// cFuncionesJson.SerializeJson(lstUsu);
                        }
                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

            return oRespuesta;
        }

        public vmRespuesta Add(vmEmpresaAdd model)
        {
            vmRespuesta oRespuesta = new vmRespuesta();
            try
            {

                string qUn = "select resultado, id  from set_empresa(@id, @empresa, @razon_social, @direccion_fiscal, @rfc, @cp, @ciudad, @logo, @url_logo, @baja)";

                using (var conDbPG = new FbConnection(CadenaFB))
                {
                    var oAdd = conDbPG.Query<vmRespuestaSet>(qUn, model).FirstOrDefault();
                    if (oAdd != null)
                    {

                        if (oAdd.resultado == 1)
                        {
                            oRespuesta.resultado = 1;

                            oRespuesta.data = oAdd.id;
                        }
                        else
                        {
                            oRespuesta.resultado = 0;
                            oRespuesta.msg = "Favor de intentar de nuevo...!";
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }



            return oRespuesta;
        }

        //public vmRespuesta Edit()
        //{
        //}

        //public vmRespuesta Delete(int id)
        //{
        //}
    }
}
