using SILCD.Models;
using SILCD.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SILCD.Controllers {
    public class CotaParlamentarController : BaseController {
        private ICotaParlamentarRepository repositorio;

        public CotaParlamentarController(ICotaParlamentarRepository _repositorio) {
            if (repositorio == null) {
                repositorio = _repositorio;
            }
        }

        public ActionResult Index() {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult JsonResultBuscarCotaParlamentarPorTipo() {
            var CotasParlamentaresPorTipo = repositorio.ListarCotaParlamentarPorTipo();
            if (CotasParlamentaresPorTipo != null) {
                List<PieChartViewModels> data = new List<PieChartViewModels>();
                foreach (CotaParlamentarViewModels cota in CotasParlamentaresPorTipo) {
                    data.Add(new PieChartViewModels() { Name = cota.txtDescricao, valor = cota.vlrDocumento });
                }
                return Json(data, JsonRequestBehavior.AllowGet);
            }
            return null;
        }

        //[AllowAnonymous]
        //[HttpGet]
        //public ActionResult Listar() {
        //    var listaCotasParlamentares = repositorio.Listar();
        //    if (listaCotasParlamentares != null) {
        //        return View(listaCotasParlamentares);
        //    }
        //    return View();
        //}
    }
}