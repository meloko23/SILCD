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

namespace SILCD.Repository.Concrete {
    public class DeputadosRepository : IDeputadosRepository, IDisposable {

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

        public List<DeputadoViewModel> ListarTodos() {
            throw new NotImplementedException();
        }

        //public List<DeputadoViewModel> ListarTodos() {

        //    if (SessionHelper.ObterDeputados() == null) {
        //        XmlNode deputadosXML = servicosDeputados.ObterDeputados();

        //        if (deputadosXML != null && deputadosXML.ChildNodes.Count > 0) {

        //            XmlDocument xmlDoc = new XmlDocument();
        //            xmlDoc.LoadXml(deputadosXML.OuterXml);
        //            XmlNodeList xmlList = xmlDoc.SelectNodes("/deputados/deputado");

        //            foreach (XmlNode nodeDeputado in xmlList) {
        //                deputado = new DeputadoViewModel();
        //                deputado.IdeCadastro = int.Parse(nodeDeputado["ideCadastro"].InnerText);
        //                deputado.Condicao = nodeDeputado["condicao"].InnerText;
        //                deputado.Matricula = nodeDeputado["matricula"].InnerText;
        //                deputado.IdParlamentar = int.Parse(nodeDeputado["idParlamentar"].InnerText);
        //                deputado.Nome = nodeDeputado["nome"].InnerText;
        //                deputado.NomeParlamentar = nodeDeputado["nomeParlamentar"].InnerText;
        //                deputado.UrlFoto = nodeDeputado["urlFoto"].InnerText;
        //                deputado.Sexo = nodeDeputado["sexo"].InnerText;
        //                deputado.Uf = nodeDeputado["uf"].InnerText;
        //                deputado.Partido = nodeDeputado["partido"].InnerText;
        //                deputado.Gabinete = nodeDeputado["gabinete"].InnerText;
        //                deputado.Telefone = nodeDeputado["fone"].InnerText;
        //                deputado.Email = nodeDeputado["email"].InnerText;

        //                deputados.Add(deputado);
        //            }

        //        }

        //        SessionHelper.ArmazenarDeputados(deputados);
        //    }

        //    return SessionHelper.ObterDeputados();

        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arquivoXml"></param>
        /// <returns></returns>
        public List<DeputadoViewModel> ListarTodosPorXml(string arquivoXml) {

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
        /// 
        /// </summary>
        /// <param name="ideCadastro"></param>
        /// <returns></returns>
        public DeputadoViewModel Buscar(int ideCadastro) {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="deputado"></param>
        /// <returns></returns>
        public DeputadoViewModel BuscarDetalhes(DeputadoViewModel deputado)
        {
            XmlNode deputadosDetalhesXML = servicosDeputados.ObterDetalhesDeputado(deputado.IdeCadastro.ToString(), null);
            foreach (XmlNode node in deputadosDetalhesXML.ChildNodes)
            {
                var dataNascimento = node["dataNascimento"].InnerText.Split('/');
                deputado.NomeProfissao = node["nomeProfissao"].InnerText;
                deputado.DataNascimento = new DateTime(int.Parse(dataNascimento[2]),
                                                        int.Parse(dataNascimento[1]),
                                                        int.Parse(dataNascimento[0]));
                return deputado;
            }
            return deputado;
        }

        public DeputadoViewModel PreencherPresencaParlamentar(DeputadoViewModel deputado, string dataIni, string dataFim)
        {
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

        public void Dispose()
        {
            servicosDeputados = null;
            deputados = null;
            deputado = null;
        }

        ~DeputadosRepository()
        {
            Dispose();
        }
    }
}