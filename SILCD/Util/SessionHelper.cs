using SILCD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SILCD.Util {
    public static class SessionHelper {
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
                HttpContext.Current.Session[Constantes.SESSION_DEPUTADOS] = (List<DeputadoViewModel>) deputados;
            }
        }
    }
}