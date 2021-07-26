using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;

namespace EcommercePrestige.Services
{
    public class MarcaServices:BaseServices<MarcaModel>, IMarcaServices
    {
        private readonly IMarcaRepository _marcaRepository;

        public MarcaServices(IMarcaRepository marcaRepository):base(marcaRepository)
        {
            _marcaRepository = marcaRepository;
        }

        public async Task<MarcaModel> GetByNameAsync(string name)
        {
            return await _marcaRepository.GetByNameAsync(name);
        }

        public async Task<IEnumerable<MarcaModel>> Filter(string termo)
        {
            return await _marcaRepository.Filter(termo);
        }

        public override async Task CreateAsync(MarcaModel marcaModel)
        {
            var verificacao =  await _marcaRepository.VerificarMarca(marcaModel.Nome);

            if (!verificacao)
            {
                await _marcaRepository.CreateAsync(marcaModel);
            }
        }
    }
}
