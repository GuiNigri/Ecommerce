using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Data.Ecommerce.Context;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace EcommercePrestige.Data.Repository
{
    public class EmpresaRepository:BaseRepository<EmpresaModel>, IEmpresaRepository
    {
        private readonly EcommerceContext _context;

        public EmpresaRepository(EcommerceContext context):base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<CidadesModel>> GetCidadesEmpresasAsync()
        {
            var result =  (from t in _context.EmpresaModel
                join pedidos in _context.PedidoModel on t.UserId equals pedidos.UsuarioModel.UserId
                join usuario in _context.UsuarioModel on t.UserId equals usuario.UserId
                where usuario.Exibir && (pedidos.Status != 2 || pedidos.Status != 6)
                select new CidadesModel
                {
                    Cidade = t.Municipio

                }).DistinctBy(x => x.Cidade).ToList();

            return result;

        }

        public async Task<IEnumerable<object>> GetBairrosByCidadesAsync(CidadesModel cidadesModel)
        {
            var result = (from t in _context.EmpresaModel
                join pedidos in _context.PedidoModel on t.UserId equals pedidos.UsuarioModel.UserId
                join usuario in _context.UsuarioModel on t.UserId equals usuario.UserId
                where t.Municipio == cidadesModel.Cidade && usuario.Exibir&& (pedidos.Status != 2 || pedidos.Status != 6)
                select new 
                {
                    Bairro = $"<option value={t.Bairro}> {t.Bairro} </option>"

                }).DistinctBy(x=>x.Bairro).ToList();

            return result;

        }

        public async Task<IEnumerable<EmpresaModel>> GetListaDeEmpresasByCidadesEBairro(string cidade, string bairro)
        {
            
            return (from empresa in _context.EmpresaModel
                join pedidos in _context.PedidoModel on empresa.UserId equals pedidos.UsuarioModel.UserId
                join usuario in _context.UsuarioModel on empresa.UserId equals usuario.UserId
                where empresa.Municipio == cidade && empresa.Bairro == bairro && usuario.Exibir && (pedidos.Status != 2 || pedidos.Status != 6)
                select new EmpresaModel(
                    empresa.Id,
                    empresa.UserId,
                    empresa.Cnpj,
                    empresa.RazaoSocial,
                    empresa.Cnae,
                    empresa.Logradouro,
                    empresa.Numero,
                    empresa.Complemento,
                    empresa.Cep,
                    empresa.Bairro,
                    empresa.Municipio,
                    empresa.Uf,
                    empresa.Telefone,
                    empresa.NomeOtica)).DistinctBy(x=>x.UserId).ToList();
        }

        public async Task<EmpresaModel> GetEmpresaByCnpj(string cnpj)
        {
            return await _context.EmpresaModel.FirstOrDefaultAsync(x => x.Cnpj == cnpj);
        }

        public async Task<EmpresaModel> GetEmpresaByUserId(string userId)
        {
            return await _context.EmpresaModel.FirstOrDefaultAsync(x => x.UserId == userId);
        }

        public async Task<IEnumerable<EmpresaModel>> FilterAsync(string termo)
        {
            return await _context.EmpresaModel.Where(x =>
                x.Bairro.Contains(termo) || x.Cnpj.Contains(termo) || x.Numero.ToString().Contains(termo) ||
                x.Complemento.Contains(termo) || x.Uf.Contains(termo) || x.Cep.Contains(termo) ||
                x.Municipio.Contains(termo) ||
                x.RazaoSocial.Contains(termo) || x.Cnae.Contains(termo) || x.Logradouro.Contains(termo) ||
                x.NomeOtica.Contains(termo) || x.Telefone.Contains(termo)).ToListAsync();
        }
    }
}
