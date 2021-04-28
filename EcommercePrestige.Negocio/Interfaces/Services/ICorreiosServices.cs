using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Services
{
    public interface ICorreiosServices
    {
        Task<IEnumerable<TrackingHistoryModel>> GetTracking(string codRastreio, int codPedido);
        Task<CorreioAddressModel> AddressByZipCode(string cep);
        Task<IEnumerable<CorreioWebServiceModel>> GetFrete(string cepDestino, int quantidadePecas, double valorTotalPedido);
    }
}
