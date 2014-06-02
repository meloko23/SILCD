using SILCD.Enum;
using SILCD.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SILCD.comissoes.br.gov.camara.www;
using System.Data;
using System.Xml;
using SILCD.Models;

namespace SILCD.Controllers {
    public class OrgaosController : BaseController {
        private IOrgaosRepository repositorio;
        private Comissoes servicosComissoes;
        private OrgaoViewModels orgao;

        public OrgaosController(IOrgaosRepository _repositorio) {
            if (repositorio == null) {
                repositorio = _repositorio;
            }
            if (servicosComissoes == null) {
                servicosComissoes = new Comissoes();
            }
        }

        public ActionResult Index() {
            var orgaos = repositorio.Listar((int)TipoDataSource.XML);
            if (orgaos != null) {
                return View(orgaos);
            }
            return View();
        }

        public ActionResult Listar() {
            var orgaos = repositorio.Listar((int)TipoDataSource.XML);
            var filtro = Request["chkFiltroTipo"];
            if (!String.IsNullOrEmpty(filtro)) {
                orgaos = orgaos.Where(o => o.Descricao.Contains(filtro)).ToList();
            }
            if (orgaos != null) {
                return PartialView("Listar", orgaos);
            }
            return View();
        }

        //TODO
        [AllowAnonymous]
        public ActionResult Detalhes(int id) {
            if (id > 0) {
                orgao = repositorio.Buscar(id);
                return View(orgao);
            }
            return View();
        }
    }
}