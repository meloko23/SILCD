using SILCD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILCD.Repository.Abstract {
    public interface ICotaParlamentarRepository {
        List<CotaParlamentarViewModels> Listar();
        List<CotaParlamentarViewModels> ListarCotaParlamentarPorReferenciaTipo();
        List<CotaParlamentarViewModels> ListarCotaParlamentarPorTipo();
    }
}
