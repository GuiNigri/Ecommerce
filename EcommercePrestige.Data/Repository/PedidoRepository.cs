using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EcommercePrestige.Data.Repository
{
    public class PedidoRepository:BaseRepository<PedidoModel>,IPedidoRepository
    {
        private readonly EcommerceContext _context;

        public PedidoRepository(EcommerceContext context) : base(context)
        {
            _context = context;
        }

        public async Task<int> CreateReturningIdAsync(PedidoModel pedidoModel)
        {
            var pedido = await _context.PedidoModel.AddAsync(pedidoModel);
            await _context.SaveChangesAsync();
            return pedido.Entity.Id;
        }

        public async Task CreateProdutosAsync(PedidoProdutosModel pedidoProdutosModel)
        {
            await _context.PedidoProdutosModel.AddAsync(pedidoProdutosModel);
            await _context.SaveChangesAsync();
        }

        public async Task CreateKitsAsync(PedidoKitModel pedidoKitModel)
        {
            await _context.PedidoKitModel.AddAsync(pedidoKitModel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProdutosAsync(PedidoProdutosModel pedidoProdutosModel)
        {
            _context.PedidoProdutosModel.Remove(pedidoProdutosModel);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteKitsAsync(PedidoKitModel pedidoKitModel)
        {
            _context.PedidoKitModel.Remove(pedidoKitModel);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<PedidoModel>> GetByUsuario(int id)
        {
            return await _context.PedidoModel.Where(x => x.UsuarioModelId == id).OrderByDescending(x=>x.DataPedido).ToListAsync();
        }

        public async Task<IEnumerable<PedidoProdutosModel>> GetProdutosByPedido(int pedido)
        {
            return  _context.PedidoProdutosModel.Where(x => x.PedidoModelId == pedido);
        }
        public async Task<PedidoProdutosModel> GetProdutosById(int id)
        {
            return await _context.PedidoProdutosModel.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<PedidoKitModel> GetKitsById(int id)
        {
            return await _context.PedidoKitModel.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<PedidoKitModel>> GetKitsByPedido(int pedido)
        {
            return _context.PedidoKitModel.Where(x => x.PedidoModelId == pedido);
        }

        public async Task<PedidoModel> GetPedido(int pedido)
        {
            return await _context.PedidoModel.FirstOrDefaultAsync(x => x.Id == pedido);
        }

        public async Task<IEnumerable<PedidoModel>> GetApprovedAsync()
        {
            return await _context.PedidoModel.Where(x => x.Status == 1).ToListAsync();
        }
        public async Task<IEnumerable<PedidoModel>> GetReprovedAsync()
        {
            return await _context.PedidoModel.Where(x => x.Status == 2).ToListAsync();
        }
        public async Task<IEnumerable<PedidoModel>> GetWaitingAsync()
        {
            return await _context.PedidoModel.Where(x => x.Status == 0).ToListAsync();
        }

        public async Task<bool> CheckPedidoUsuario(int usuarioId, int pedido)
        {
            return await _context.PedidoModel.AnyAsync(x => x.UsuarioModelId == usuarioId && x.Id == pedido);
        }

        public async Task<bool> CheckTrackingCode(string trackingCode, int pedido)
        {
            return await _context.PedidoModel.AnyAsync(x => x.Rastreio == trackingCode && x.Id == pedido);
        }

        public async Task<IEnumerable<PedidoModel>> FilterAsync(string termo, int status)
        {
            return await _context.PedidoModel.Where(x => x.Id.ToString().Contains(termo) || x.UsuarioModel.NomeCompleto.Contains(termo) ||
                                                         x.Rastreio.Contains(termo) || x.FormaDePagamento.Contains(termo) || x.ValorTotal.ToString().Contains(termo) && (status < 0 || x.Status == status)).ToListAsync();
        }

        public async Task UpdateProdutosPedidoAsync(PedidoProdutosModel pedidoProdutosModel)
        {
            _context.PedidoProdutosModel.Update(pedidoProdutosModel);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateKitsPedidoAsync(PedidoKitModel pedidoKitModel)
        {
            _context.PedidoKitModel.Update(pedidoKitModel);
            await _context.SaveChangesAsync();
        }
        public async Task<bool> CheckIfExistOrderByUserAsync(int idUsuario)
        {
            return await _context.PedidoModel.AnyAsync(x => x.UsuarioModelId == idUsuario);
        }

        public async Task<IEnumerable<PedidoModel>> GetPedidosDespachados()
        {
            return await _context.PedidoModel.Where(x => x.Status == 3).ToListAsync();
        }

    }
}
