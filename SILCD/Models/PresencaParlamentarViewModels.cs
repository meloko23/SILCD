using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SILCD.Models
{
    public class PresencaParlamentarViewModels
    {
        private DateTime data;
        private string frequenciaNoDia;
        private string justificativa;
        private List<SessaoViewModels> sessoes;

        public DateTime Data
        {
            get { return data; }
            set { data = value; }
        }

        public string FrequenciaNoDia
        {
            get { return frequenciaNoDia; }
            set { frequenciaNoDia = value; }
        }

        public string Justificativa
        {
            get { return justificativa; }
            set { justificativa = value; }
        }

        public List<SessaoViewModels> Sessoes
        {
            get { return sessoes; }
            set { sessoes = value; }
        }
    }
}