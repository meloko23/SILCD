using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SILCD.Models
{
    public class SessaoViewModels
    {
        private string descricao;
        private string frequencia;

        public string Descricao
        {
            get { return descricao; }
            set { descricao = value; }
        }

        public string Frequencia
        {
            get { return frequencia; }
            set { frequencia = value; }
        }
    }
}