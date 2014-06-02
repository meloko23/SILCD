using SILCD.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SILCD.Repository.Abstract {
    public interface IOrgaosRepository {
        List<OrgaoViewModels> Listar(int tipoDataSource);

        OrgaoViewModels Buscar(int idOrgao);
    }
}
