using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Infrastructure;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;

namespace EcommercePrestige.Services
{
    public class CorreioServices:ICorreiosServices
    {
        private readonly ICorreiosInfrastructure _correiosInfrastructure;
        private readonly IPedidoRepository _pedidoRepository;

        public CorreioServices(ICorreiosInfrastructure correiosInfrastructure, IPedidoRepository pedidoRepository)
        {
            _correiosInfrastructure = correiosInfrastructure;
            _pedidoRepository = pedidoRepository;
        }
        public async Task<IEnumerable<TrackingHistoryModel>> GetTracking(string codRastreio, int codPedido)
        {
            var checagem = await _pedidoRepository.CheckTrackingCode(codRastreio, codPedido);

            if (checagem)
            {
                return await _correiosInfrastructure.PackageTracking(codRastreio);
            }

            return null;
        }

        public async Task<CorreioAddressModel> AddressByZipCode(string cep)
        {
            return await _correiosInfrastructure.AddressByZipCode(cep);
        }

        public async Task<IEnumerable<CorreioWebServiceModel>> GetFrete(string cepDestino, int quantidadePecas, double valorTotalPedido)
        {
           return await _correiosInfrastructure.CalcularFrete(cepDestino,quantidadePecas,valorTotalPedido);
        }
    }
}
