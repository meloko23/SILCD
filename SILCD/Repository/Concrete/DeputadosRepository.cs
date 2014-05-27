using SILCD.br.gov.camara.www;
using SILCD.Models;
using SILCD.Repository.Abstract;
using SILCD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace SILCD.Repository.Concrete {
    public class DeputadosRepository : IDeputadosRepository, IDisposable {

        //private Deputados servicosDeputados;
        private List<DeputadoViewModel> deputados = new List<DeputadoViewModel>();
        private DeputadoViewModel deputado;
        private List<PresencaParlamentarViewModels> listaPresencaParlamentar;
        private List<SessaoViewModels> listaSessaoParlamentar;

        //public DeputadosRepository() {
        //    if (servicosDeputados == null) {
        //        servicosDeputados = new Deputados();
        //    }
        //}

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

        public DeputadoViewModel Buscar(int ideCadastro) {
            throw new NotImplementedException();
        }

        // TODO
        public DeputadoViewModel BuscarDetalhes(int ideCadastro)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load("http://www.camara.gov.br/SitCamaraWS/Deputados.asmx/ObterDetalhesDeputado?ideCadastro=" + ideCadastro.ToString() + "&numLegislatura=");

            XmlNodeList nodes = xml.SelectNodes("/Deputados/Deputado");

            foreach (XmlNode node in nodes)
            {

            }
            return null;
        }

        // TODO
        public DeputadoViewModel PreencherPresencaParlamentar(DeputadoViewModel deputado, string dataIni, string dataFim)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load("http://www.camara.gov.br/SitCamaraWS/sessoesreunioes.asmx/ListarPresencasParlamentar?dataIni=" + dataIni + "&dataFim=" + dataFim + "&numMatriculaParlamentar=" + deputado.Matricula);

            XmlNodeList nodesPresencaParlamentar = xml.SelectNodes("/parlamentar/diasDeSessoes2/dia");

            listaPresencaParlamentar = new List<PresencaParlamentarViewModels>();
            PresencaParlamentarViewModels presencaParlamentar;
            foreach (XmlNode xmlNodePresencaParlamentar in nodesPresencaParlamentar)
            {
                listaSessaoParlamentar = new List<SessaoViewModels>();
                SessaoViewModels sessao;
                XmlNodeList nodesSessoes = xml.SelectNodes("/sessoes/sessao");

                foreach (XmlNode xmlNodeSessao in nodesSessoes)
                {
                    sessao = new SessaoViewModels();
                    sessao.Descricao = xmlNodeSessao["descricao"].InnerText;
                    sessao.Frequencia = xmlNodeSessao["frequencia"].InnerText;

                    listaSessaoParlamentar.Add(sessao);
                }

                presencaParlamentar = new PresencaParlamentarViewModels();
                presencaParlamentar.Data = Convert.ToDateTime(xmlNodePresencaParlamentar["data"].InnerText);
                presencaParlamentar.FrequenciaNoDia = xmlNodePresencaParlamentar["frequencianoDia"].InnerText;
                presencaParlamentar.Justificativa = xmlNodePresencaParlamentar["justificativa"].InnerText;
                presencaParlamentar.Sessoes = listaSessaoParlamentar;

                listaPresencaParlamentar.Add(presencaParlamentar);
            }

            return deputado;
        }

        public void Dispose()
        {
            //servicosDeputados = null;
            deputados = null;
            deputado = null;
        }

        ~DeputadosRepository()
        {
            Dispose();
        }
    }
}