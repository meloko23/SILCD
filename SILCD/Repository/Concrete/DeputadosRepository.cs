using SILCD.br.gov.camara.www;
using SILCD.sessoesReunioes.br.gov.camara.www;
using SILCD.Models;
using SILCD.Repository.Abstract;
using SILCD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using SILCD.Helper;
using System.Data;
using SILCD.Enum;
using SILCD.Resources;

namespace SILCD.Repository.Concrete {
    public class DeputadosRepository : BaseRepository, IDeputadosRepository {

        private Deputados servicosDeputados;
        private SessoesReunioes servicosSessoesReunioes;
        private List<DeputadoViewModel> deputados = new List<DeputadoViewModel>();
        private DeputadoViewModel deputado;
        private List<PresencaParlamentarViewModels> listaPresencaParlamentar;
        private List<SessaoViewModels> listaSessaoParlamentar;

        public DeputadosRepository() {

            if (servicosDeputados == null) {
                servicosDeputados = new Deputados();
            }

            if (servicosSessoesReunioes == null) {
                servicosSessoesReunioes = new SessoesReunioes();
            }

        }

        public List<DeputadoViewModel> Listar(int tipoDataSource) {
            List<DeputadoViewModel> retorno = null;
            switch (tipoDataSource) {

                case (int)TipoDataSource.DATA_BASE:
                    retorno = ListarPorBD();
                    break;
                case (int)TipoDataSource.XML:
                    retorno = ListarPorXml();
                    break;
                case (int)TipoDataSource.WEBSERVICE:
                    retorno = ListarPorWebService();
                    break;
            }
            return retorno;
        }

        IEnumerable<DeputadoViewModel> IDeputadosRepository.List {
            get {
                return Listar((int)TipoDataSource.XML);
            }
        }

        public DeputadoViewModel Find(int id) {
            if (id > 0) {
                return Listar((int)TipoDataSource.XML).ToList().Find(d => d.IdeCadastro.Equals(id));
            } else {
                return null;
            }
        }

        public DeputadoViewModel Buscar(int ideCadastro, int tipoDataSource) {
            try {
                DeputadoViewModel deputado = Listar(tipoDataSource).Single(d => d.IdeCadastro.Equals(ideCadastro));
                return deputado;
            } catch {
                throw new Exception(Messages.RecordNotFound);
            }
        }

        public DeputadoViewModel BuscarDetalhes(DeputadoViewModel deputado) {
            XmlNode deputadosDetalhesXML = servicosDeputados.ObterDetalhesDeputado(deputado.IdeCadastro.ToString(), null);
            foreach (XmlNode node in deputadosDetalhesXML.ChildNodes) {
                var dataNascimento = node["dataNascimento"].InnerText.Split('/');
                deputado.NomeProfissao = node["nomeProfissao"].InnerText;
                deputado.DataNascimento = new DateTime(int.Parse(dataNascimento[2]),
                                                        int.Parse(dataNascimento[1]),
                                                        int.Parse(dataNascimento[0]));
                return deputado;
            }
            return deputado;
        }

        public DeputadoViewModel PreencherPresencaParlamentar(DeputadoViewModel deputado, string dataIni, string dataFim) {
            XmlNode deputadosPresencaParlamentarXML = servicosSessoesReunioes.ListarPresencasParlamentar(dataIni, dataFim, deputado.Matricula);
            XmlNodeList nodesPresencaParlamentar = deputadosPresencaParlamentarXML.SelectNodes("/diasDeSessoes2/dia");

            listaPresencaParlamentar = new List<PresencaParlamentarViewModels>();
            PresencaParlamentarViewModels presencaParlamentar;

            foreach (XmlNode xmlNodePresencaParlamentar in nodesPresencaParlamentar) {
                listaSessaoParlamentar = new List<SessaoViewModels>();
                SessaoViewModels sessao;
                XmlNodeList nodesSessoes = xmlNodePresencaParlamentar.SelectNodes("sessoes/sessao");

                foreach (XmlNode xmlNodeSessao in nodesSessoes) {
                    sessao = new SessaoViewModels();
                    sessao.Descricao = xmlNodeSessao["descricao"].InnerText;
                    sessao.Frequencia = xmlNodeSessao["frequencia"].InnerText;

                    listaSessaoParlamentar.Add(sessao);
                }

                var dataPresencaParlamentar = xmlNodePresencaParlamentar["data"].InnerText.Split('/');
                presencaParlamentar = new PresencaParlamentarViewModels();
                presencaParlamentar.Data = new DateTime(int.Parse(dataPresencaParlamentar[2]),
                                                        int.Parse(dataPresencaParlamentar[1]),
                                                        int.Parse(dataPresencaParlamentar[0]));
                presencaParlamentar.FrequenciaNoDia = xmlNodePresencaParlamentar["frequencianoDia"].InnerText;
                presencaParlamentar.Justificativa = xmlNodePresencaParlamentar["justificativa"].InnerText;
                presencaParlamentar.Sessoes = listaSessaoParlamentar;

                listaPresencaParlamentar.Add(presencaParlamentar);
            }

            deputado.ListaPresencaParlamentar = listaPresencaParlamentar;
            return deputado;
        }

        /// <summary>
        /// Lista do banco
        /// </summary>
        /// <returns></returns>
        List<DeputadoViewModel> ListarPorBD() {
            return null;
            //try {
            //    DeputadoViewModel deputado;
            //    List<DeputadoViewModel> deputados = new List<DeputadoViewModel>();

            //    db.CommandText = " SELECT  ideCadastro ," +
            //                     "        condicao ," +
            //                     "        matricula ," +
            //                     "        idParlamentar ," +
            //                     "        nome ," +
            //                     "        nomeParlamentar ," +
            //                     "        urlFoto ," +
            //                     "        sexo ," +
            //                     "        uf ," +
            //                     "        partido ," +
            //                     "        gabinete ," +
            //                     "        anexo ," +
            //                     "        fone ," +
            //                     "        email" +
            //                     " FROM dbo.Deputados";
            //    DataSet dsResultado = db.GetDataSet();
            //    if (dsResultado != null) {
            //        foreach (DataRow deputadoRow in dsResultado.Tables[0].Rows) {
            //            deputado = new DeputadoViewModel();
            //            deputado.IdeCadastro = int.Parse(deputadoRow["ideCadastro"].ToString());
            //            deputado.Condicao = deputadoRow["condicao"].ToString();
            //            deputado.Matricula = deputadoRow["matricula"].ToString();
            //            deputado.IdParlamentar = int.Parse(deputadoRow["idParlamentar"].ToString());
            //            deputado.Nome = deputadoRow["nome"].ToString();
            //            deputado.NomeParlamentar = deputadoRow["nomeParlamentar"].ToString();
            //            deputado.UrlFoto = deputadoRow["urlFoto"].ToString();
            //            deputado.Sexo = deputadoRow["sexo"].ToString();
            //            deputado.Uf = deputadoRow["uf"].ToString();
            //            deputado.Partido = deputadoRow["partido"].ToString();
            //            deputado.Gabinete = deputadoRow["gabinete"].ToString();
            //            deputado.Anexo = deputadoRow["anexo"].ToString();
            //            deputado.Telefone = deputadoRow["fone"].ToString();
            //            deputado.Email = deputadoRow["email"].ToString();

            //            deputados.Add(deputado);
            //        }
            //    }

            //    return deputados;
            //} catch {
            //    return null;
            //}
        }

        /// <summary>
        /// Lista do XML
        /// </summary>
        /// <returns></returns>
        List<DeputadoViewModel> ListarPorXml() {

            var arquivoXml = HttpContext.Current.Server.MapPath(Constantes.XML_DEPUTADOS);

            if (!String.IsNullOrEmpty(arquivoXml)) {

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(arquivoXml);
                XmlNodeList xmlList = xmlDoc.SelectNodes("/deputados/deputado");

                foreach (XmlNode nodeDeputado in xmlList) {
                    deputado = new DeputadoViewModel();
                    deputado.IdeCadastro = int.Parse(nodeDeputado["ideCadastro"].InnerText);
                    deputado.Condicao = nodeDeputado["condicao"].InnerText;
                    deputado.Matricula = nodeDeputado["matricula"].InnerText;
                    deputado.IdParlamentar = int.Parse(nodeDeputado["idParlamentar"].InnerText);
                    deputado.Nome = nodeDeputado["nome"].InnerText;
                    deputado.NomeParlamentar = nodeDeputado["nomeParlamentar"].InnerText;
                    deputado.UrlFoto = nodeDeputado["urlFoto"].InnerText;
                    deputado.Sexo = nodeDeputado["sexo"].InnerText;
                    deputado.Uf = nodeDeputado["uf"].InnerText;
                    deputado.Partido = nodeDeputado["partido"].InnerText;
                    deputado.Gabinete = nodeDeputado["gabinete"].InnerText;
                    deputado.Telefone = nodeDeputado["fone"].InnerText;
                    deputado.Email = nodeDeputado["email"].InnerText;

                    deputados.Add(deputado);
                }

                SessionHelper.ArmazenarDeputados(deputados);
            }

            return SessionHelper.ObterDeputados();
        }

        /// <summary>
        /// Lista do Webservice
        /// </summary>
        /// <returns></returns>
        List<DeputadoViewModel> ListarPorWebService() {

            if (SessionHelper.ObterDeputados() == null) {
                XmlNode deputadosXML = servicosDeputados.ObterDeputados();

                if (deputadosXML != null && deputadosXML.ChildNodes.Count > 0) {

                    XmlDocument xmlDoc = new XmlDocument();
                    xmlDoc.LoadXml(deputadosXML.OuterXml);
                    XmlNodeList xmlList = xmlDoc.SelectNodes("/deputados/deputado");

                    foreach (XmlNode nodeDeputado in xmlList) {
                        deputado = new DeputadoViewModel();
                        deputado.IdeCadastro = int.Parse(nodeDeputado["ideCadastro"].InnerText);
                        deputado.Condicao = nodeDeputado["condicao"].InnerText;
                        deputado.Matricula = nodeDeputado["matricula"].InnerText;
                        deputado.IdParlamentar = int.Parse(nodeDeputado["idParlamentar"].InnerText);
                        deputado.Nome = nodeDeputado["nome"].InnerText;
                        deputado.NomeParlamentar = nodeDeputado["nomeParlamentar"].InnerText;
                        deputado.UrlFoto = nodeDeputado["urlFoto"].InnerText;
                        deputado.Sexo = nodeDeputado["sexo"].InnerText;
                        deputado.Uf = nodeDeputado["uf"].InnerText;
                        deputado.Partido = nodeDeputado["partido"].InnerText;
                        deputado.Gabinete = nodeDeputado["gabinete"].InnerText;
                        deputado.Telefone = nodeDeputado["fone"].InnerText;
                        deputado.Email = nodeDeputado["email"].InnerText;

                        deputados.Add(deputado);
                    }

                }

                SessionHelper.ArmazenarDeputados(deputados);
            }

            return SessionHelper.ObterDeputados();

        }

        ~DeputadosRepository() {
            servicosDeputados = null;
            deputados = null;
            deputado = null;
        }
    }
}