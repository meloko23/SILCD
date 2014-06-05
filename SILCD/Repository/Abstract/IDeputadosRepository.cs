﻿using SILCD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILCD.Repository.Abstract {
    public interface IDeputadosRepository {

        IEnumerable<DeputadoViewModel> List { get; }

        DeputadoViewModel Find(int id);

        List<DeputadoViewModel> Listar();

        DeputadoViewModel Buscar(int ideCadastro);

        DeputadoViewModel BuscarDetalhes(DeputadoViewModel deputado);

        DeputadoViewModel PreencherPresencaParlamentar(DeputadoViewModel deputado, string dataIni, string dataFim);
        
        List<CotaParlamentarViewModels> ListarCotaParlamentar(int ideCadastro);

        List<CotaParlamentarViewModels> ListarCotaParlamentarPorReferenciaTipo(int ideCadastro);

        List<CotaParlamentarViewModels> ListarCotaParlamentarPorTipo(int ideCadastro);
    }
}
