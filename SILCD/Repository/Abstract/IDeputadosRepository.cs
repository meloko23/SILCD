using SILCD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILCD.Repository.Abstract {
    public interface IDeputadosRepository {
        List<DeputadoViewModel> ListarTodos();

        List<DeputadoViewModel> ListarTodosPorXml(string arquivoXml);

        DeputadoViewModel Buscar(int ideCadastro);

        DeputadoViewModel BuscarDetalhes(int ideCadastro);

        DeputadoViewModel PreencherPresencaParlamentar(DeputadoViewModel deputado, string dataIni, string dataFim);
    }
}
