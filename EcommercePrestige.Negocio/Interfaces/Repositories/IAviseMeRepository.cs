using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Repositories
{
    public interface IAviseMeRepository:IBaseRepository<AviseMeModel>
    {
        Task<IEnumerable<AviseMeModel>> VerificarReferenciasSolicitadas(int idReferencia);
    }
}
