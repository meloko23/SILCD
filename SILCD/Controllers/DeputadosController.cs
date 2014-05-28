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

namespace SILCD.Controllers
{
    public class DeputadosController : BaseController
    {

        private IDeputadosRepository repositorio;
        private List<DeputadoViewModel> deputados = new List<DeputadoViewModel>();
        private List<PresencaParlamentarViewModels> listaPresencaParlamentar;
        private List<SessaoViewModels> listaSessaoParlamentar;

        public DeputadosController(IDeputadosRepository _repositorio)
        {
            if (repositorio == null)
            {
                repositorio = _repositorio;
            }
        }

        public ActionResult Index()
        {
            try
            {
                //deputados = repositorio.ListarTodos();
                deputados = repositorio.ListarTodosPorXml(Server.MapPath(Constantes.XML_DEPUTADOS));
                string txtPesquisa = Request["txtPesquisa"];
                if (!String.IsNullOrEmpty(txtPesquisa))
                {
                    try
                    {
                        var _deputados = deputados.Where(d => d.Nome.ToUpper().Contains(txtPesquisa.ToUpper()) || d.Uf.Contains(txtPesquisa) || d.Partido.Contains(txtPesquisa)).ToList();
                        deputados = (List<DeputadoViewModel>)_deputados;
                    }
                    catch
                    {

                    }
                }

                if (deputados != null)
                {
                    return View(deputados);
                }
                return View(new List<DeputadoViewModel>());
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        public ActionResult Detalhe(int id = 0)
        {
            try
            {
                var deputados = repositorio.ListarTodosPorXml(Server.MapPath(Constantes.XML_DEPUTADOS)).ToList();
                var deputado = deputados.Find(d => d.IdeCadastro.Equals(id));
                BuscarDetalhes(deputado);
                PreencherPresencaParlamentar(deputado, null, null);
                if (deputado == null)
                {
                    return HttpNotFound();
                }
                return View(deputado);
            }
            catch (Exception erro)
            {
                throw new Exception(erro.Message);
            }
        }

        private DeputadoViewModel BuscarDetalhes(DeputadoViewModel deputado)
        {
            if (deputado != null && deputado.IdeCadastro > 0)
            {
                return repositorio.BuscarDetalhes(deputado);
            }
            else
            {
                return null;
            }
        }

        private DeputadoViewModel PreencherPresencaParlamentar(DeputadoViewModel deputado, string dataIni, string dataFim)
        {
            if (deputado == null) {
                throw new Exception("Informe o deputado para obter a lista de presença.");
            }
            
            if (String.IsNullOrEmpty(dataIni)) {
                dataIni = String.Format("{0:dd/MM/yyyy}", new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
            }

            if (String.IsNullOrEmpty(dataFim)) {
                dataFim = String.Format("{0:dd/MM/yyyy}", DateTime.Now);
            }

            return repositorio.PreencherPresencaParlamentar(deputado, dataIni, dataFim);
        }

        ~DeputadosController()
        {
            deputados = null;
            repositorio = null;
        }

        public ActionResult DistribuicaoPorUF()
        {
            var deputados = repositorio.ListarTodosPorXml(Server.MapPath(Constantes.XML_DEPUTADOS)).ToList();
            deputados = deputados.GroupBy(d => d.Uf)
                                 .Select(n => new { Text = n.Key, Value = n.Count() });

            return View();
        }
    }
}