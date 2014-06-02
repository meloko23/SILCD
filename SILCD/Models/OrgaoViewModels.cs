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
        private DeputadoViewModel presidente;
        private DeputadoViewModel primeiroVicePresidente;
        private DeputadoViewModel segundoVicePresidente;
        private DeputadoViewModel terceiroVicePresidente;
        private DeputadoViewModel relator;
        private List<DeputadoViewModel> membros;


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

        public TipoOrgaoViewModels TipoOrgao {
            get;
            set;
        }

        public DeputadoViewModel Presidente {
            get { return presidente; }
            set { presidente = value; }
        }

        public DeputadoViewModel PrimeiroVicePresidente {
            get { return primeiroVicePresidente; }
            set { primeiroVicePresidente = value; }
        }

        public DeputadoViewModel SegundoVicePresidente {
            get { return segundoVicePresidente; }
            set { segundoVicePresidente = value; }
        }

        public DeputadoViewModel TerceiroVicePresidente {
            get { return terceiroVicePresidente; }
            set { terceiroVicePresidente = value; }
        }

        public DeputadoViewModel Relator {
            get { return relator; }
            set { relator = value; }
        }

        public List<DeputadoViewModel> Membros {
            get { return membros; }
            set { membros = value; }
        }

        public void AdicionaMembro(DeputadoViewModel membro) {
            if (membros != null) {
                membros.Add(membro);
            }
        }

    }
}