using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SILCD.Models {
    public class CotaParlamentarViewModels {
        public int Id { get; set; }
        public string txNomeParlamentar { get; set; }
        public int? nuCarteiraParlamentar { get; set; }
        public int? nuLegislatura { get; set; }
        public string sgUF { get; set; }
        public string sgPartido { get; set; }
        public int? codLegislatura { get; set; }
        public int? numSubCota { get; set; }
        public string txtDescricao { get; set; }
        public int? numEspecificacaoSubCota { get; set; }
        public string txtDescricaoEspecificacao { get; set; }
        public string txtFornecedor { get; set; }
        public string txtCNPJCPF { get; set; }
        public string txtNumero { get; set; }
        public int? indTipoDocumento { get; set; }
        public DateTime? datEmissao { get; set; }
        public decimal vlrDocumento { get; set; }
        public decimal? vlrGlosa { get; set; }
        public decimal? vlrLiquido { get; set; }
        public int? numMes { get; set; }
        public int? numAno { get; set; }
        public int? numParcela { get; set; }
        public string txtPassageiro { get; set; }
        public string txtTrecho { get; set; }
        public int? numLote { get; set; }
        public int? numRessarcimento { get; set; }
        public int? ideCadastro { get; set; }
    }
}