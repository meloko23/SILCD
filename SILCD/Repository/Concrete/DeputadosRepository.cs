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
using System.Data.SqlClient;
using System.Configuration;
using DbFactory.Implementation;

namespace SILCD.Repository.Concrete {
    public class DeputadosRepository : BaseRepository, IDeputadosRepository {

        private Deputados servicosDeputados;
        private SessoesReunioes servicosSessoesReunioes;
        private List<DeputadoViewModel> deputados = new List<DeputadoViewModel>();
        private DeputadoViewModel deputado;
        private List<PresencaParlamentarViewModels> listaPresencaParlamentar;
        private List<SessaoViewModels> listaSessaoParlamentar;
        private List<CotaParlamentarViewModels> listaDeCotaParlamentar = null;       

        /// <summary>
        /// 
        /// </summary>
        public DeputadosRepository() {

            if (servicosDeputados == null) {
                servicosDeputados = new Deputados();
            }

            if (servicosSessoesReunioes == null) {
                servicosSessoesReunioes = new SessoesReunioes();
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tipoDataSource"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        IEnumerable<DeputadoViewModel> IDeputadosRepository.List {
            get {
                return Listar((int)TipoDataSource.XML);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DeputadoViewModel Find(int id) {
            if (id > 0) {
                return Listar((int)TipoDataSource.XML).ToList().Find(d => d.IdeCadastro.Equals(id));
            } else {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ideCadastro"></param>
        /// <param name="tipoDataSource"></param>
        /// <returns></returns>
        public DeputadoViewModel Buscar(int ideCadastro, int tipoDataSource) {
            try {
                DeputadoViewModel deputado = Listar(tipoDataSource).Single(d => d.IdeCadastro.Equals(ideCadastro));
                return deputado;
            } catch {
                throw new Exception(Messages.RecordNotFound);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deputado"></param>
        /// <returns></returns>
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deputado"></param>
        /// <param name="dataIni"></param>
        /// <param name="dataFim"></param>
        /// <returns></returns>
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
        /// 
        /// </summary>
        /// <param name="ideCadastro"></param>
        /// <returns></returns>
        public List<CotaParlamentarViewModels> ListarCotaParlamentar(int ideCadastro) {
            if (ideCadastro > 0) {
                DbConnection db = null;
                try {
                    db = new DbConnection("connectionString");
                    db.CommandText = " SELECT * "+
                                     " FROM [dbo].[CotaParlamentar] "+
                                     " WHERE ideCadastro = @ideCadastro " +
                                     " ORDER BY numAno, numMes, txtDescricao, vlrDocumento, vlrGlosa, vlrLiquido ";
                    db.AddParameter(new SqlParameter("@ideCadastro", ideCadastro));
                    DataSet dsRetorno = db.GetDataSet();
                    if (dsRetorno != null && dsRetorno.Tables[0].Rows.Count > 0) {
                        listaDeCotaParlamentar = new List<CotaParlamentarViewModels>();
                        foreach (DataRow cotaParlamentarRow in dsRetorno.Tables[0].Rows) {
                            listaDeCotaParlamentar.Add(new CotaParlamentarViewModels {
                                Id = int.Parse(cotaParlamentarRow["Id"].ToString()),
                                txNomeParlamentar = cotaParlamentarRow["txNomeParlamentar"].ToString(),
                                nuCarteiraParlamentar = cotaParlamentarRow["nuCarteiraParlamentar"] == DBNull.Value ? 0 : (int?)cotaParlamentarRow["nuCarteiraParlamentar"],
                                nuLegislatura = int.Parse(cotaParlamentarRow["nuLegislatura"].ToString()),
                                sgUF = cotaParlamentarRow["sgUF"].ToString(),
                                sgPartido = cotaParlamentarRow["sgPartido"].ToString(),
                                codLegislatura = cotaParlamentarRow["codLegislatura"] == DBNull.Value ? 0 : (int?)cotaParlamentarRow["codLegislatura"],
                                numSubCota = int.Parse(cotaParlamentarRow["numSubCota"].ToString()),
                                txtDescricao = cotaParlamentarRow["txtDescricao"].ToString(),
                                numEspecificacaoSubCota = int.Parse(cotaParlamentarRow["numEspecificacaoSubCota"].ToString()),
                                txtDescricaoEspecificacao = cotaParlamentarRow["txtDescricaoEspecificacao"].ToString(),
                                txtFornecedor = cotaParlamentarRow["txtFornecedor"].ToString(),
                                txtCNPJCPF = cotaParlamentarRow["txtCNPJCPF"].ToString(),
                                txtNumero = cotaParlamentarRow["txtNumero"].ToString(),
                                indTipoDocumento = int.Parse(cotaParlamentarRow["indTipoDocumento"].ToString()),
                                datEmissao = cotaParlamentarRow["datEmissao"] == DBNull.Value ? new DateTime(1900, 1, 1) : (DateTime?)cotaParlamentarRow["datEmissao"],
                                vlrDocumento = decimal.Parse(cotaParlamentarRow["vlrDocumento"].ToString()),
                                vlrGlosa = decimal.Parse(cotaParlamentarRow["vlrGlosa"].ToString()),
                                vlrLiquido = decimal.Parse(cotaParlamentarRow["vlrLiquido"].ToString()),
                                numMes = int.Parse(cotaParlamentarRow["numMes"].ToString()),
                                numAno = int.Parse(cotaParlamentarRow["numAno"].ToString()),
                                numParcela = int.Parse(cotaParlamentarRow["numParcela"].ToString()),
                                txtPassageiro = cotaParlamentarRow["txtPassageiro"].ToString(),
                                txtTrecho = cotaParlamentarRow["txtTrecho"].ToString(),
                                numLote = int.Parse(cotaParlamentarRow["numLote"].ToString()),
                                numRessarcimento = int.Parse(cotaParlamentarRow["numRessarcimento"].ToString()),
                                ideCadastro = int.Parse(cotaParlamentarRow["ideCadastro"].ToString())
                            });
                        }
                    }
                } catch {
                    throw new Exception();
                } finally {
                    db.Dispose();
                }

            }
            return listaDeCotaParlamentar;
        }

        public List<CotaParlamentarViewModels> ListarCotaParlamentarPorReferenciaTipo(int ideCadastro) {
            if (ideCadastro > 0) {
                DbConnection db = null;
                try {
                    db = new DbConnection("connectionString");
                    db.CommandText = " SELECT numAno, numMes, UPPER(txtDescricao) [txtDescricao], SUM(vlrDocumento) [vlrDocumento], SUM(vlrGlosa) [vlrGlosa], SUM(vlrLiquido) [vlrLiquido] " +
                                     " FROM [dbo].[CotaParlamentar] " +
                                     " WHERE ideCadastro = @ideCadastro " +
                                     " GROUP BY numAno, numMes, UPPER(txtDescricao) " +
                                     " ORDER BY numAno, numMes, UPPER(txtDescricao) " ;

                    db.AddParameter(new SqlParameter("@ideCadastro", ideCadastro));
                    DataSet dsRetorno = db.GetDataSet();
                    if (dsRetorno != null && dsRetorno.Tables[0].Rows.Count > 0) {
                        listaDeCotaParlamentar = new List<CotaParlamentarViewModels>();
                        foreach (DataRow cotaParlamentarRow in dsRetorno.Tables[0].Rows) {
                            listaDeCotaParlamentar.Add(new CotaParlamentarViewModels {
                                txtDescricao = cotaParlamentarRow["txtDescricao"].ToString(),
                                vlrDocumento = decimal.Parse(cotaParlamentarRow["vlrDocumento"].ToString()),
                                vlrGlosa = decimal.Parse(cotaParlamentarRow["vlrGlosa"].ToString()),
                                vlrLiquido = decimal.Parse(cotaParlamentarRow["vlrLiquido"].ToString()),
                                numMes = int.Parse(cotaParlamentarRow["numMes"].ToString()),
                                numAno = int.Parse(cotaParlamentarRow["numAno"].ToString())
                            });
                        }
                    }
                } catch {
                    throw new Exception();
                } finally {
                    db.Dispose();
                }

            }
            return listaDeCotaParlamentar;
        }

        public List<CotaParlamentarViewModels> ListarCotaParlamentarPorTipo(int ideCadastro) {
            if (ideCadastro > 0) {
                DbConnection db = null;
                try {
                    db = new DbConnection("connectionString");
                    db.CommandText = " SELECT UPPER(txtDescricao) [txtDescricao], SUM(vlrDocumento) [vlrDocumento], SUM(vlrGlosa) [vlrGlosa], SUM(vlrLiquido) [vlrLiquido] " +
                                     " FROM [dbo].[CotaParlamentar] " +
                                     " WHERE ideCadastro = @ideCadastro " +
                                     " GROUP BY UPPER(txtDescricao) " +
                                     " ORDER BY UPPER(txtDescricao) " ;

                    db.AddParameter(new SqlParameter("@ideCadastro", ideCadastro));
                    DataSet dsRetorno = db.GetDataSet();
                    if (dsRetorno != null && dsRetorno.Tables[0].Rows.Count > 0) {
                        listaDeCotaParlamentar = new List<CotaParlamentarViewModels>();
                        foreach (DataRow cotaParlamentarRow in dsRetorno.Tables[0].Rows) {
                            listaDeCotaParlamentar.Add(new CotaParlamentarViewModels {
                                txtDescricao = cotaParlamentarRow["txtDescricao"].ToString(),
                                vlrDocumento = decimal.Parse(cotaParlamentarRow["vlrDocumento"].ToString()),
                                vlrGlosa = decimal.Parse(cotaParlamentarRow["vlrGlosa"].ToString()),
                                vlrLiquido = decimal.Parse(cotaParlamentarRow["vlrLiquido"].ToString())
                            });
                        }
                    }
                } catch {
                    throw new Exception();
                } finally {
                    db.Dispose();
                }

            }
            return listaDeCotaParlamentar;
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