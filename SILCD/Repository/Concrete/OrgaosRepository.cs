using SILCD.comissoes.br.gov.camara.www;
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

        OrgaoViewModels orgao = null;
        List<OrgaoViewModels> orgaos;

        private Comissoes servicosComissoes;

        public OrgaosRepository() {
            if (servicosComissoes == null) {
                servicosComissoes = new Comissoes();
            }
        }

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
                    DataRow[] dtTipoOrgao = dsTiposOrgaos.Tables[0].Select("id = '" + orgao.IdTipodeOrgao.ToString() + "'", "id ASC");

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



        public OrgaoViewModels Buscar(int idOrgao) {

            XmlNode xmlNodeMembros = servicosComissoes.ObterMembros(idOrgao);

            if (xmlNodeMembros.ChildNodes.Count > 0) {
                XmlNodeList xmlNodeListPresidente = xmlNodeMembros.SelectNodes("membros/Presidente");
                XmlNodeList xmlNodeListPrimeiroVicePresidente = xmlNodeMembros.SelectNodes("membros/PrimeiroVice-Presidente");
                XmlNodeList xmlNodeListSegundoVicePresidente = xmlNodeMembros.SelectNodes("membros/SegundoVice-Presidente");
                XmlNodeList xmlNodeListTerceiroVicePresidente = xmlNodeMembros.SelectNodes("membros/TerceiroVice-Presidente");
                XmlNodeList xmlNodeListRelator = xmlNodeMembros.SelectNodes("membros/Relator");
                XmlNodeList xmlNodeListMembros = xmlNodeMembros.SelectNodes("membros/membro");

                orgao = Listar((int)TipoDataSource.XML).Single(o => o.Id.Equals(idOrgao));

                if (xmlNodeListPresidente.Count > 0)
                    orgao.Presidente = new DeputadoViewModel {
                        Nome = xmlNodeListPresidente[0]["nome"].InnerText,
                        Partido = xmlNodeListPresidente[0]["partido"].InnerText,
                        Uf = xmlNodeListPresidente[0]["uf"].InnerText,
                        Condicao = xmlNodeListPresidente[0]["situacao"].InnerText

                    };

                if (xmlNodeListPrimeiroVicePresidente.Count > 0)
                    orgao.PrimeiroVicePresidente = new DeputadoViewModel {
                        Nome = xmlNodeListPrimeiroVicePresidente[0]["nome"].InnerText,
                        Partido = xmlNodeListPrimeiroVicePresidente[0]["partido"].InnerText,
                        Uf = xmlNodeListPrimeiroVicePresidente[0]["uf"].InnerText,
                        Condicao = xmlNodeListPrimeiroVicePresidente[0]["situacao"].InnerText

                    };

                if (xmlNodeListSegundoVicePresidente.Count > 0)
                    orgao.SegundoVicePresidente = new DeputadoViewModel {
                        Nome = xmlNodeListSegundoVicePresidente[0]["nome"].InnerText,
                        Partido = xmlNodeListSegundoVicePresidente[0]["partido"].InnerText,
                        Uf = xmlNodeListSegundoVicePresidente[0]["uf"].InnerText,
                        Condicao = xmlNodeListSegundoVicePresidente[0]["situacao"].InnerText

                    };

                if (xmlNodeListTerceiroVicePresidente.Count > 0)
                    orgao.TerceiroVicePresidente = new DeputadoViewModel {
                        Nome = xmlNodeListTerceiroVicePresidente[0]["nome"].InnerText,
                        Partido = xmlNodeListTerceiroVicePresidente[0]["partido"].InnerText,
                        Uf = xmlNodeListTerceiroVicePresidente[0]["uf"].InnerText,
                        Condicao = xmlNodeListTerceiroVicePresidente[0]["situacao"].InnerText

                    };

                if (xmlNodeListRelator.Count > 0)
                    orgao.Relator = new DeputadoViewModel {
                        Nome = xmlNodeListRelator[0]["nome"].InnerText,
                        Partido = xmlNodeListRelator[0]["partido"].InnerText,
                        Uf = xmlNodeListRelator[0]["uf"].InnerText,
                        Condicao = xmlNodeListRelator[0]["situacao"].InnerText

                    };

                if (xmlNodeListMembros.Count > 0)
                    orgao.Membros = new List<DeputadoViewModel>();

                foreach (XmlNode _membro in xmlNodeListMembros) {
                    orgao.AdicionaMembro(new DeputadoViewModel {
                        Nome = _membro["nome"].InnerText,
                        Partido = _membro["partido"].InnerText,
                        Uf = _membro["uf"].InnerText,
                        Condicao = _membro["situacao"].InnerText
                    });
                }
            }

            return orgao;
        }
    }

}