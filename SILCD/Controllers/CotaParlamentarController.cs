using SILCD.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SILCD.Controllers {
    public class CotaParlamentarController : Controller {
        private ICotaParlamentarRepository repositorio;

        public CotaParlamentarController(ICotaParlamentarRepository _repositorio) {
            if (repositorio == null) {
                repositorio = _repositorio;
            }
        }

        //
        // GET: /CotaParlamentar/
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index() {
            return View();
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Listar() {
            var listaCotasParlamentares = repositorio.Listar();
            if (listaCotasParlamentares != null) {
                return View(listaCotasParlamentares);
            }
            return View();
        }
    }
}