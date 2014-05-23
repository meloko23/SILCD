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

namespace SILCD.Controllers {
    public class DeputadosController : BaseController {

        private List<DeputadoViewModel> deputados = new List<DeputadoViewModel>();
        private IDeputadosRepository repositorio;

        public DeputadosController(IDeputadosRepository _repositorio) {
            if (repositorio == null) {
                repositorio = _repositorio;
            }
        }
        
        public ActionResult Index() {
            try {
                deputados = repositorio.ListarTodos();
                if (deputados != null) {
                    return View(deputados);   
                }
                return View(new List<DeputadoViewModel>());
            } catch (Exception erro) {
                throw new Exception(erro.Message);
            }
        }
    }
}