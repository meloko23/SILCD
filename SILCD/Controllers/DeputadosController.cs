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

                //BuscarCotaParlamentar();
                //readXmlCotaParlamentar();

                if (deputados != null) {
                    return View(deputados);
                }

                return View(new List<DeputadoViewModel>());

            } catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }

        public ActionResult ListarPorNome() {
            try {
                var deputados = repositorio.List;
                string txtPesquisa = Request["txtPesquisa"];

                if (!String.IsNullOrEmpty(txtPesquisa)) {
                    var _deputados = deputados.Where(d => d.Nome.ToUpper().Contains(txtPesquisa.ToUpper()) ||
                                                          d.Uf.Contains(txtPesquisa) ||
                                                          d.Partido.Contains(txtPesquisa)).ToList();
                    deputados = (List<DeputadoViewModel>)_deputados;
                }

                if (deputados != null) {
                    return PartialView("ListarPorNome", deputados);
                }

                return PartialView();

            } catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }

        public ActionResult Detalhe(int id = 0) {
            try {
                var deputado = repositorio.Buscar(id);
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

        public ActionResult CotaParlamentar(int id = 0) {
            try {
                if (id > 0) {
                    var CotasParlamentares = repositorio.ListarCotaParlamentar(id);
                    BuscarCotaParlamentarPorReferenciaTipo(id);
                    BuscarCotaParlamentarPorTipo(id);
                    if (CotasParlamentares == null) {
                        return HttpNotFound();
                    }
                    return View(CotasParlamentares);
                }
                return View();
            } catch {
                throw new Exception(Messages.RecordNotFound);
            }
        }

        private void BuscarCotaParlamentarPorReferenciaTipo(int id = 0) {
            if (id > 0) {
                var CotasParlamentaresPorReferenciaTipo = repositorio.ListarCotaParlamentarPorReferenciaTipo(id);
                if (CotasParlamentaresPorReferenciaTipo != null) {
                    TempData["CotaParlamentarPorReferenciaTipo"] = CotasParlamentaresPorReferenciaTipo;
                }
            }
        }

        private void BuscarCotaParlamentarPorTipo(int id = 0) {
            if (id > 0) {
                var CotasParlamentaresPorTipo = repositorio.ListarCotaParlamentarPorTipo(id);
                if (CotasParlamentaresPorTipo != null) {
                    TempData["CotaParlamentarPorTipo"] = CotasParlamentaresPorTipo;
                }
            }
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult JsonResultBuscarCotaParlamentarPorTipo(int id = 0) {
            if (id > 0) {
                var CotasParlamentaresPorTipo = repositorio.ListarCotaParlamentarPorTipo(id);
                if (CotasParlamentaresPorTipo != null) {
                    List<PieChartViewModels> data = new List<PieChartViewModels>();
                    foreach (CotaParlamentarViewModels cota in CotasParlamentaresPorTipo) {
                        data.Add(new PieChartViewModels() { Name = cota.txtDescricao, valor = cota.vlrDocumento });
                    }
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            return null;
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
            var deputados = repositorio.Listar().ToList();
            var query = from d in deputados
                        group d by d.Uf into g
                        select new { Uf = g.Key, UfCount = g.Count() };

            SessionHelper.GravarDistribuicaoPorUF(query);

        }

        private void BuscarDistribuicaoPorPartido() {
            var deputados = repositorio.Listar().ToList();
            var query = from d in deputados
                        group d by d.Partido into g
                        select new { Partido = g.Key, PartidoCount = g.Count() };

            SessionHelper.GravarDistribuicaoPorPartido(query);
        }

        private List<CotaParlamentarViewModels> readXmlCotaParlamentar() {
            List<CotaParlamentarViewModels> listaDeCotas = new List<CotaParlamentarViewModels>();
            try {
                string XmlFileUrl = @"C:\Users\fernando.faria\Desktop\AnoAnterior.xml";
                CotaParlamentarViewModels cotaParlamentar = null;
                using (XmlReader reader = new XmlTextReader(XmlFileUrl)) {
                    while (reader.Read()) {
                        if (reader.NodeType == XmlNodeType.Element) {
                            switch (reader.Name) {
                                case "DESPESA":
                                    if (cotaParlamentar != null) {
                                        listaDeCotas.Add(cotaParlamentar);
                                    }
                                    cotaParlamentar = new CotaParlamentarViewModels();
                                    break;
                                case "txNomeParlamentar":
                                    cotaParlamentar.txNomeParlamentar = reader.ReadContentAsString();
                                    break;
                                //case "nuCarteiraParlamentar":
                                //    cotaParlamentar.nuCarteiraParlamentar = int.Parse(reader.ReadContentAsString());
                                //    break;
                                //case "nuLegislatura":
                                //    cotaParlamentar.nuLegislatura = int.Parse(reader.ReadContentAsString());
                                //    break;
                                case "sgUF":
                                    cotaParlamentar.sgUF = reader.ReadContentAsString();
                                    break;
                                case "sgPartido":
                                    cotaParlamentar.sgPartido = reader.ReadContentAsString();
                                    break;
                                //case "codLegislatura":
                                //    cotaParlamentar.codLegislatura = int.Parse(reader.ReadContentAsString());
                                //    break;
                                //case "numSubCota":
                                //    cotaParlamentar.numSubCota = int.Parse(reader.ReadContentAsString());
                                //    break;
                                case "txtDescricao":
                                    cotaParlamentar.txtDescricao = reader.ReadContentAsString();
                                    break;
                                //case "numEspecificacaoSubCota":
                                //    cotaParlamentar.numEspecificacaoSubCota = int.Parse(reader.ReadContentAsString());
                                //    break;
                                case "txtDescricaoEspecificacao":
                                    cotaParlamentar.txtDescricaoEspecificacao = reader.ReadContentAsString();
                                    break;
                                case "txtFornecedor":
                                    cotaParlamentar.txtFornecedor = reader.ReadContentAsString();
                                    break;
                                case "txtCNPJCPF":
                                    cotaParlamentar.txtCNPJCPF = reader.ReadContentAsString();
                                    break;
                                case "txtNumero":
                                    cotaParlamentar.txtNumero = reader.ReadContentAsString();
                                    break;
                                //case "indTipoDocumento":
                                //    cotaParlamentar.indTipoDocumento = int.Parse(reader.ReadContentAsString());
                                //    break;
                                //case "datEmissao":
                                //    cotaParlamentar.datEmissao = DateTime.Parse(reader.ReadContentAsString());
                                //    break;
                                //case "vlrDocumento":
                                //    cotaParlamentar.vlrDocumento = decimal.Parse(reader.ReadContentAsString());
                                //    break;
                                //case "vlrGlosa":
                                //    cotaParlamentar.vlrGlosa = decimal.Parse(reader.ReadContentAsString());
                                //    break;
                                //case "vlrLiquido":
                                //    cotaParlamentar.vlrLiquido = decimal.Parse(reader.ReadContentAsString());
                                //    break;
                                //case "numMes":
                                //    cotaParlamentar.numMes = int.Parse(reader.ReadContentAsString());
                                //    break;
                                //case "numAno":
                                //    cotaParlamentar.numAno = int.Parse(reader.ReadContentAsString());
                                //    break;
                                //case "numParcela":
                                //    cotaParlamentar.numParcela = int.Parse(reader.ReadContentAsString());
                                //    break;
                                case "txtPassageiro":
                                    cotaParlamentar.txtPassageiro = reader.ReadContentAsString();
                                    break;
                                case "txtTrecho":
                                    cotaParlamentar.txtTrecho = reader.ReadContentAsString();
                                    break;
                                //case "numLote":
                                //    cotaParlamentar.numLote = int.Parse(reader.ReadContentAsString());
                                //    break;
                                //case "numRessarcimento":
                                //    cotaParlamentar.numRessarcimento = int.Parse(reader.ReadContentAsString());
                                //    break;
                                //case "ideCadastro":
                                //    cotaParlamentar.ideCadastro = int.Parse(reader.ReadContentAsString());
                                //    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            } catch (Exception erro) {
                throw new Exception(erro.Message);
            }

            return listaDeCotas;
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