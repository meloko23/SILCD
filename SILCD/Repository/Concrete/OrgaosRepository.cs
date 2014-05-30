using SILCD.Enum;
using SILCD.Models;
using SILCD.Repository.Abstract;
using SILCD.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Xml;

namespace SILCD.Repository.Concrete {

    public class OrgaosRepository : BaseRepository, IOrgaosRepository {

        OrgaoViewModels orgao;
        List<OrgaoViewModels> orgaos;

        public List<OrgaoViewModels> Listar(int tipoDataSource) {
            List<OrgaoViewModels> retorno = null;
            switch (tipoDataSource) {

                case (int)TipoDataSource.DATA_BASE:
                    retorno = null;
                    break;
                case (int)TipoDataSource.XML:
                    retorno = ListarPorXml();
                    break;
                case (int)TipoDataSource.WEBSERVICE:
                    retorno = null;
                    break;
            }
            return retorno;
        }

        /// <summary>
        /// Lista do XML
        /// </summary>
        /// <returns></returns>
        List<OrgaoViewModels> ListarPorXml() {

            var arquivoXmlOrgaos = HttpContext.Current.Server.MapPath(Constantes.XML_ORGAOS);
            var arquivoXmlTiposOrgaos = HttpContext.Current.Server.MapPath(Constantes.XML_TIPOS_ORGAOS);

            DataSet dsOrgaos = new DataSet();
            dsOrgaos.ReadXml(arquivoXmlOrgaos);

            DataSet dsTiposOrgaos = new DataSet();
            dsTiposOrgaos.ReadXml(arquivoXmlTiposOrgaos);

            if (dsOrgaos != null && dsOrgaos.Tables[0] != null && dsOrgaos.Tables[0].Rows.Count > 0) {

                orgaos = new List<OrgaoViewModels>();

                foreach (DataRow orgaoRow in dsOrgaos.Tables[0].Rows) {
                    orgao = new OrgaoViewModels();
                    orgao.Id = int.Parse(orgaoRow["id"].ToString());
                    orgao.IdTipodeOrgao = int.Parse(orgaoRow["idTipodeOrgao"].ToString());
                    orgao.Sigla = orgaoRow["sigla"].ToString();
                    orgao.Descricao = orgaoRow["descricao"].ToString();

                    //var dtTipoOrgao = dsTiposOrgaos.Tables[0].Select("id = " + orgao.IdTipodeOrgao);
                    //var dtTipoOrgao = dsTiposOrgaos.Tables[0].Select("id = " + orgao.IdTipodeOrgao, "id ASC");
                    DataRow[] dtTipoOrgao = dsTiposOrgaos.Tables[0].Select("id = '" + orgao.IdTipodeOrgao.ToString()+ "'", "id ASC");

                    TipoOrgaoViewModels tipoOrgao;
                    foreach (DataRow tipo in dtTipoOrgao) {
                        tipoOrgao = new TipoOrgaoViewModels();
                        tipoOrgao.Id = int.Parse(tipo["id"].ToString());
                        tipoOrgao.Descricao = tipo["descricao"].ToString();

                        orgao.TipoOrgao = tipoOrgao;
                    }

                    orgaos.Add(orgao);
                }
            }

            return orgaos;
        }

    }

}