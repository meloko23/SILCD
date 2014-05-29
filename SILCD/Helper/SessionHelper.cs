using SILCD.Models;
using SILCD.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SILCD.Helper {
    public abstract class SessionHelper {

        public static List<DeputadoViewModel> ObterDeputados() {
            try {
                if (HttpContext.Current.Session[Constantes.SESSION_DEPUTADOS] != null)
                    return (List<DeputadoViewModel>)HttpContext.Current.Session[Constantes.SESSION_DEPUTADOS];
                else
                    return null;
            } catch {
                return null;
            }
        }

        public static void ArmazenarDeputados(object deputados) {
            if (deputados != null && deputados is List<DeputadoViewModel>) {
                HttpContext.Current.Session[Constantes.SESSION_DEPUTADOS] = (List<DeputadoViewModel>)deputados;
            }
        }

        public static object BuscarDistribuicaoPorUF() {
            try {
                if (HttpContext.Current.Session[Constantes.SESSION_DEPUTADOS_DISTRIBUICAO_UF] != null)
                    return (object)HttpContext.Current.Session[Constantes.SESSION_DEPUTADOS_DISTRIBUICAO_UF];
                else
                    return null;
            } catch {
                return null;
            }
        }

        public static void GravarDistribuicaoPorUF(object distribuicaoPorUF) {
            if (distribuicaoPorUF != null) {
                HttpContext.Current.Session[Constantes.SESSION_DEPUTADOS_DISTRIBUICAO_UF] = distribuicaoPorUF;
            }
        }

        public static object BuscarDistribuicaoPorPartido() {
            try {
                if (HttpContext.Current.Session[Constantes.SESSION_DEPUTADOS_DISTRIBUICAO_PARTIDO] != null)
                    return (object)HttpContext.Current.Session[Constantes.SESSION_DEPUTADOS_DISTRIBUICAO_PARTIDO];
                else
                    return null;
            } catch {
                return null;
            }
        }

        public static void GravarDistribuicaoPorPartido(object distribuicaoPorPartido) {
            if (distribuicaoPorPartido != null) {
                HttpContext.Current.Session[Constantes.SESSION_DEPUTADOS_DISTRIBUICAO_PARTIDO] = distribuicaoPorPartido;
            }
        }

    }
}