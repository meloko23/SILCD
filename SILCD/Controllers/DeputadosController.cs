using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using SILCD.br.gov.camara.www;
using SILCD.Models;
using SILCD.Util;
using System.Data;
using SILCD.Repository.Abstract;
using SILCD.Helper;
using SILCD.Enum;
using SILCD.Resources;
using System.Data.SqlClient;
using System.Configuration;

namespace SILCD.Controllers {
    public class DeputadosController : BaseController {

        private IDeputadosRepository repositorio;
        private List<DeputadoViewModel> deputados = new List<DeputadoViewModel>();

        public DeputadosController(IDeputadosRepository _repositorio) {
            if (repositorio == null) {
                repositorio = _repositorio;
            }
        }

        public ActionResult Index() {
            try {
                var deputados = repositorio.List;
                string txtPesquisa = Request["txtPesquisa"];

                if (!String.IsNullOrEmpty(txtPesquisa)) {
                    try {
                        var _deputados = deputados.Where(d => d.Nome.ToUpper().Contains(txtPesquisa.ToUpper()) ||
                                                              d.Uf.Contains(txtPesquisa) ||
                                                              d.Partido.Contains(txtPesquisa)).ToList();
                        deputados = (List<DeputadoViewModel>)_deputados;
                    } catch (Exception erro) {
                        throw new Exception(erro.Message);
                    }
                }

                if (deputados != null) {
                    return View(deputados);
                }

                return View(new List<DeputadoViewModel>());

            } catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }

        public ActionResult Detalhe(int id = 0) {
            try {
                var deputado = repositorio.Buscar(id, (int)TipoDataSource.XML);
                BuscarDetalhes(deputado);
                PreencherPresencaParlamentar(deputado, null, null); //TODO (está com erro, analisar problema)
                if (deputado == null) {
                    return HttpNotFound();
                }
                return View(deputado);
            } catch {
                throw new Exception(Messages.RecordNotFound);
            }
        }

        private DeputadoViewModel BuscarDetalhes(DeputadoViewModel deputado) {
            if (deputado != null && deputado.IdeCadastro > 0) {
                return repositorio.BuscarDetalhes(deputado);
            } else {
                return null;
            }
        }

        private DeputadoViewModel PreencherPresencaParlamentar(DeputadoViewModel deputado, string dataIni, string dataFim) {
            if (deputado == null) {
                throw new Exception("Informe o deputado para obter a lista de presença.");
            }

            if (String.IsNullOrEmpty(dataIni)) {
                //dataIni = String.Format("{0:dd/MM/yyyy}", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
                dataIni = String.Format("{0:dd/MM/yyyy}", new DateTime(DateTime.Now.Year, 1, 1));
            }

            if (String.IsNullOrEmpty(dataFim)) {
                dataFim = String.Format("{0:dd/MM/yyyy}", DateTime.Now);
            }

            return repositorio.PreencherPresencaParlamentar(deputado, dataIni, dataFim);
        }

        private void BuscarDistribuicaoPorUF() {
            var deputados = repositorio.Listar((int)TipoDataSource.XML).ToList();
            var query = from d in deputados
                        group d by d.Uf into g
                        select new { Uf = g.Key, UfCount = g.Count() };

            SessionHelper.GravarDistribuicaoPorUF(query);

        }

        private void BuscarDistribuicaoPorPartido() {
            var deputados = repositorio.Listar((int)TipoDataSource.XML).ToList();
            var query = from d in deputados
                        group d by d.Partido into g
                        select new { Partido = g.Key, PartidoCount = g.Count() };

            SessionHelper.GravarDistribuicaoPorPartido(query);
        }

        // TODO
        private void BuscarCotaParlamentar() {
            DataSet dsCotaParlamentar = new DataSet();
            dsCotaParlamentar.ReadXml(Server.MapPath("~/App_GlobalResources/CotaParlamentar2014.xml"));
            DataTable dtCotaParlamentar = dsCotaParlamentar.Tables["DESPESA"];
            bool bInsert = BulkInsertDataTable("CotaParlamentarNova", dtCotaParlamentar);
            if (bInsert) {

            }
        }

        private bool BulkInsertDataTable(string tableName, DataTable dataTable) {
            bool isSuccuss;
            try {
                //SqlConnection SqlConnectionObj = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionString"].ToString());
                SqlConnection SqlConnectionObj = new SqlConnection(ConfigurationManager.ConnectionStrings["connectionStringAzure"].ToString());
                SqlConnectionObj.Open();
                //SqlBulkCopy bulkCopy = new SqlBulkCopy(SqlConnectionObj, SqlBulkCopyOptions.TableLock | SqlBulkCopyOptions.FireTriggers | SqlBulkCopyOptions.UseInternalTransaction, null);
                SqlBulkCopy bulkCopy = new SqlBulkCopy(SqlConnectionObj, SqlBulkCopyOptions.Default | SqlBulkCopyOptions.UseInternalTransaction, null);
                bulkCopy.BulkCopyTimeout = 999999;
                bulkCopy.DestinationTableName = tableName;
                bulkCopy.WriteToServer(dataTable);                
                isSuccuss = true;
            } catch (Exception ex) {
                isSuccuss = false;
            }
            return isSuccuss;
        }

        public ActionResult DistribuicaoPorUF() {
            BuscarDistribuicaoPorUF();
            return View(SessionHelper.BuscarDistribuicaoPorUF());
        }

        public ActionResult DistribuicaoPorPartido() {
            BuscarDistribuicaoPorPartido();
            return View(SessionHelper.BuscarDistribuicaoPorPartido());
        }

        ~DeputadosController() {
            deputados = null;
            repositorio = null;
        }
    }
}