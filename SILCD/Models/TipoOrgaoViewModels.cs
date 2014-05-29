using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SILCD.Models {
    public class TipoOrgaoViewModels : BaseViewModels {

        private int id;
        private string descricao;

        public int Id {
            get { return id; }
            set { id = value; }
        }

        public string Descricao {
            get { return descricao; }
            set { descricao = value; }
        }

    }
}