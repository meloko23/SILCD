using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SILCD.Models {
    public class DeputadoViewModel {

        private int ideCadastro;
        private string condicao;
        private string matricula;
        private int idParlamentar;
        private string nome;
        private string nomeParlamentar;
        private string urlFoto;
        private string sexo;
        private string uf;
        private string partido;
        private string gabinete;
        private string telefone;
        private string email;

        [Display(Name="Id do Parlamentar")]
        public int IdParlamentar {
            get { return idParlamentar; }
            set { idParlamentar = value; }
        }

        public string Nome {
            get { return nome; }
            set { nome = value; }
        }

        [Display(Name = "Nome Parlamentar")]
        public string NomeParlamentar {
            get { return nomeParlamentar; }
            set { nomeParlamentar = value; }
        }
        
        public string UrlFoto {
            get { return urlFoto; }
            set { urlFoto = value; }
        }

        public string Sexo {
            get { return sexo; }
            set { sexo = value; }
        }

        public string Uf {
            get { return uf; }
            set { uf = value; }
        }

        public string Partido {
            get { return partido; }
            set { partido = value; }
        }
        public string Gabinete {
            get { return gabinete; }
            set { gabinete = value; }
        }

        public string Telefone {
            get { return telefone; }
            set { telefone = value; }
        }

        [Display(Name = "E-mail")]
        public string Email {
            get { return email; }
            set { email = value; }
        }

        public int IdeCadastro {
            get { return ideCadastro; }
            set { ideCadastro = value; }
        }

        [Display(Name = "Condição")]
        public string Condicao {
            get { return condicao; }
            set { condicao = value; }
        }

        [Display(Name = "Matrícula")]
        public string Matricula {
            get { return matricula; }
            set { matricula = value; }
        }

    }

}