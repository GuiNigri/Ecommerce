using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Infrastructure
{
    public interface ICorreiosInfrastructure
    {
        Task<IEnumerable<TrackingHistoryModel>> PackageTracking(string rastreio);
        Task<CorreioAddressModel> AddressByZipCode(string cep);
        Task<bool> VerificarSeEntregue(string rastreio);
        Task<IEnumerable<CorreioWebServiceModel>> CalcularFrete(string cepDestino, int quantidadePecas, double valorTotalPedido);
    }
}
