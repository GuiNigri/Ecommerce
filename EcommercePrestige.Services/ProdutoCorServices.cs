using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;

namespace EcommercePrestige.Services
{
    public class ProdutoCorServices:BaseServices<ProdutoCorModel>, IProdutoCorServices
    {
        private readonly IProdutoCorRepository _produtoCorRepository;
        private readonly IAviseMeRepository _aviseMeRepository;
        private readonly IEmailSenderServices _emailSenderServices;

        public ProdutoCorServices(IProdutoCorRepository produtoCorRepository, IAviseMeRepository aviseMeRepository, IEmailSenderServices emailSenderServices) : base(produtoCorRepository)
        {
            _produtoCorRepository = produtoCorRepository;
            _aviseMeRepository = aviseMeRepository;
            _emailSenderServices = emailSenderServices;
        }

        public async Task<IEnumerable<ProdutoCorModel>> GetAllGroupedAsync(string statusAtivacao)
        {
            return await _produtoCorRepository.GetAllGroupedAsync(statusAtivacao);
        }

        public async Task<IEnumerable<ProdutoCorModel>> GetByProdutoAsync(int id, string statusAtivacao)
        {
            return await _produtoCorRepository.GetByProdutoAsync(id, statusAtivacao);
        }
        public async Task<(IEnumerable<ProdutoCorModel>,bool)> GetListByKitAsync(string kitName, string statusAtivacao)
        {
            var lista = await _produtoCorRepository.GetListByKitAsync(kitName, statusAtivacao);

            var verificarEstoqueLista = false;

            if (lista.Any())
            {
                verificarEstoqueLista = await _produtoCorRepository.VerificarEstoqueNegativo(lista.ToList());
            }


            return (lista.ToList(), verificarEstoqueLista);
        }

        public async Task<ProdutoCorModel> GetCorByProdutoAsync(int corId, int prodId, string statusAtivacao)
        {
            return await _produtoCorRepository.GetCorByProdutoAsync(corId, prodId, statusAtivacao);
        }

        public async Task<bool> VerificarEstoque(int prodId, int corId, int quantidade, string statusAtivacao)
        {
            return await _produtoCorRepository.VerificarEstoque(prodId, corId, quantidade, statusAtivacao);
        }

        public async Task<(bool,string)> AddListaCor(ProdutoCorModel produtoCorModel)
        {
            var verificacao =
                await _produtoCorRepository.VerificarSeCorExiste(produtoCorModel.ProdutoModelId, produtoCorModel.CorModelId);

            if (!verificacao)
            {
                await _produtoCorRepository.CreateAsync(produtoCorModel);
                return (true, "Adicionado com sucesso");
            }

            return (false, "Essa cor já esta cadastrada!");
        }

        public async Task<IEnumerable<ProdutoCorModel>> RetornarListaDeCorDoProduto(int id, string statusAtivacao)
        {
            return await _produtoCorRepository.GetByProdutoAsync(id, statusAtivacao);
        }

        public async Task<bool> VerificarSeCorExiste(int idProd, int idCor)
        {
            return await _produtoCorRepository.VerificarSeCorExiste(idProd, idCor);
        }

        public async Task UpdateAsync(int id)
        {
            var produtoCorModel = await _produtoCorRepository.GetByIdAsync(id);

            produtoCorModel.SetStatusAtivacao(produtoCorModel.StatusAtivacao == "AT" ? "IN" : "AT");

            await _produtoCorRepository.UpdateAsync(produtoCorModel);
        }

        public async Task AlterarEstoqueAsync(int id, int quantidade)
        {
            var produtoCorModel = await _produtoCorRepository.GetByIdAsync(id);

            produtoCorModel.SetEstoque(quantidade);

            await _produtoCorRepository.UpdateAsync(produtoCorModel);

            if (quantidade > 0)
            {
                var listaAviseMe = await _aviseMeRepository.VerificarReferenciasSolicitadas(produtoCorModel.Id);

                if (listaAviseMe.Any())
                {
                    foreach (var item in listaAviseMe)
                    {
                        await EnviarEmailAviseMe(produtoCorModel.ProdutoModel.Referencia, produtoCorModel.CodigoInterno,
                            item.Email);
                    }
                    
                }
            }
        }

        private async Task EnviarEmailAviseMe(string armação, string cor, string email)
        {
            const string subject = "Sua armação já está disponível";

            var mensagemEmail = $"Olá, <br> <br>" +
                                $"A armação {armação} - {cor}, já está disponível em nossa loja virtual! <br> <br> <br>" +
                                $"Equipe de relacionamento com o cliente <br>" +
                                $"Prestige do Brasil";

            await _emailSenderServices.SendEmailAsync(email, subject, mensagemEmail, null, null);
        }

        public async Task AlterarAtivacaoKitNoProduto(int idCor, string kit)
        {
            var produtoCorModel = await _produtoCorRepository.GetByIdAsync(idCor);

            switch (kit)
            {
                case "gold":
                    produtoCorModel.SetPedidoGold(!produtoCorModel.PedidoGold);
                    break;
                case "silver":
                    produtoCorModel.SetPedidoSilver(!produtoCorModel.PedidoSilver);
                    break;
                case "basic":
                    produtoCorModel.SetPedidoBasic(!produtoCorModel.PedidoBasic);
                    break;
            }

            await _produtoCorRepository.UpdateAsync(produtoCorModel);
        }
    }
}
