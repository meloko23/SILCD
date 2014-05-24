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
    public class DeputadosRepository : IDeputadosRepository {

        private Deputados servicosDeputados;
        private List<DeputadoViewModel> deputados = new List<DeputadoViewModel>();
        private DeputadoViewModel deputado;

        public DeputadosRepository() {
            if (servicosDeputados == null) {
                servicosDeputados = new Deputados();
            }
        }

        public List<DeputadoViewModel> ListarTodos() {

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
    }
}