using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SILCD.Models {
    public class OrgaoViewModels : BaseViewModels {
        
        private int id;
        private int idTipodeOrgao;
        private string sigla;
        private string descricao;

        public int Id {
            get { return id; }
            set { id = value; }
        }

        public int IdTipodeOrgao {
            get { return idTipodeOrgao; }
            set { idTipodeOrgao = value; }
        }

        public string Sigla {
            get { return sigla; }
            set { sigla = value; }
        }

        public string Descricao {
            get { return descricao; }
            set { descricao = value; }
        }

        public virtual TipoOrgaoViewModels TipoOrgaoViewModels {
            get { return TipoOrgaoViewModels; }
            set { TipoOrgaoViewModels = value; }
        }

    }
}