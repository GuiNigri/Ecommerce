using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Repositories
{
    public interface IConsultaCnpjAwsApi
    {
        Task<EmpresaApiModel> consultarCNPJ(string cnpj);
    }
}
