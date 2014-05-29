﻿using SILCD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILCD.Repository.Abstract {
    public interface IDeputadosRepository {

        List<DeputadoViewModel> Listar(int tipoDataSource);

        DeputadoViewModel Buscar(int ideCadastro, int tipoDataSource);

        DeputadoViewModel BuscarDetalhes(DeputadoViewModel deputado);

        DeputadoViewModel PreencherPresencaParlamentar(DeputadoViewModel deputado, string dataIni, string dataFim);
    }
}
