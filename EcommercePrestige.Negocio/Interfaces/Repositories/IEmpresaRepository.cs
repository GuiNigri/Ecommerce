using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Repositories
{
    public interface IEmpresaRepository:IBaseRepository<EmpresaModel>
    {
        Task<IEnumerable<CidadesModel>> GetCidadesEmpresasAsync();
        Task<IEnumerable<object>> GetBairrosByCidadesAsync(CidadesModel cidadesModel);
        Task<IEnumerable<EmpresaModel>> GetListaDeEmpresasByCidadesEBairro(string cidade, string bairro);
        Task<EmpresaModel> GetEmpresaByCnpj(string cnpj);
        Task<EmpresaModel> GetEmpresaByUserId(string userId);
        Task<IEnumerable<EmpresaModel>> FilterAsync(string termo);
    }
}
