using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SILCD.Util {
    public static class Constantes {
        // Sessões
        public const string SESSION_DEPUTADOS = "session_deputados";
        public const string SESSION_DEPUTADOS_DISTRIBUICAO_UF = "session_deputados_distribuicao_uf";
        public const string SESSION_DEPUTADOS_DISTRIBUICAO_PARTIDO = "session_deputados_distribuicao_partido";

        // Erros
        public const string TEMPDATA_ERROR = "error";

        // Constantes dos resources
        public const string XML_DEPUTADOS = "~/App_GlobalResources/ObterDeputados.xml";
    }
}