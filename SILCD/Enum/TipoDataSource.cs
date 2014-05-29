using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SILCD.Enum {
    public enum TipoDataSource : byte {
        DATA_BASE = 1,
        XML,
        WEBSERVICE
    }
}