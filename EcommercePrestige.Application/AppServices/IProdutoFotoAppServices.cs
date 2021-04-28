using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Http;

namespace EcommercePrestige.Application.AppServices
{
    public interface IProdutoFotoAppServices
    {
        Task<IEnumerable<ProdutoFotoViewModel>> GetByProdutoAsync(int id);
        Task<ProdutoFotoViewModel> GetFotosToKitAsync(int id);
        Task<(bool,string)> AddFotoLista(IFormFile file, ProdutoFotoInputModel produtoFotoInputModel, string statusAtivacao);
        Task RemoveListaFoto(int id);
        Task<IEnumerable<ProdutoFotoInputModel>> RetornarListaFotoInput(int idProd);
        Task CreateAsync(int idProduto, string statusAtivacao);
        Task<bool> CheckFotoAndPrincipal(int idProd);
        Task UpdateAsync(int idFoto);
        Task<IEnumerable<ProdutoFotoViewModel>> GetFotosByCorAsync(int id);
        Task<ProdutoFotoViewModel> GetFotosByCorIndexAsync(int id);
    }
}
