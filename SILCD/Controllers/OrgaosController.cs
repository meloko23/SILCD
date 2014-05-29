using SILCD.Enum;
using SILCD.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SILCD.Controllers
{
    public class OrgaosController : BaseController
    {
        private IOrgaosRepository repositorio;

        public OrgaosController(IOrgaosRepository _repositorio) {
            if (repositorio == null) {
                repositorio = _repositorio;
            }
        }

        public ActionResult Index()
        {
            var orgaos = repositorio.Listar((int)TipoDataSource.XML);
            if (orgaos != null) {
                return View(orgaos);
            }
            return View();
        }
	}
}