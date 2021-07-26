using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Application.ViewModel;

namespace EcommercePrestige.Application.AppServices
{
    public interface IEmpresaAppServices
    {
        Task<(EmpresaViewModel,string)> GetDadosEmpresaAsync(string cnpj);
        Task Create(EmpresaViewModel empresaViewModel, UsuarioViewModel usuarioViewModel);
        Task<IEnumerable<CidadesViewModel>> GetCidadesEmpresasAsync();
        Task<IEnumerable<object>> GetBairrosByCidadesAsync(CidadesViewModel cidadesViewModel);
        Task<IEnumerable<EmpresaViewModel>> GetListaDeEmpresasByCidadesEBairro(string cidade, string bairro);
        Task<EmpresaViewModel> GetEmpresaByCnpj(string cnpj);
        Task<EmpresaViewModel> GetEmpresaByUserId(string userId);
        Task<IEnumerable<EmpresaViewModel>> GetAllAsync();
        Task<IEnumerable<EmpresaViewModel>> FilterAsync(string termo);
        Task<EmpresaViewModel> GetByIdAsync(int id);
        Task UpdateAsync(EmpresaViewModel empresaViewModel);
        Task DeleteAsync(int id);
    }
}
