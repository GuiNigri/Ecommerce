using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcommercePrestige.Application.AppServices;
using EcommercePrestige.Application.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace EcommercePrestige.Apresentacao.Controllers
{
    [Authorize(policy: "Admin")]
    public class AdminProdutoController : AdministracaoController
    {
        private readonly IMarcaAppServices _marcaAppServices;
        private readonly IMaterialAppServices _materialAppServices;
        private readonly IProdutoAppServices _produtoAppServices;
        private readonly IProdutoCorAppServices _produtoCorAppServices;
        private readonly IProdutoFotoAppServices _produtoFotoAppServices;
        private readonly ICorAppServices _corAppServices;

        public AdminProdutoController(IMarcaAppServices marcaAppServices, IMaterialAppServices materialAppServices, 
            IProdutoAppServices produtoAppServices,IProdutoCorAppServices produtoCorAppServices, 
            IProdutoFotoAppServices produtoFotoAppServices,ICorAppServices corAppServices)
        {
            _marcaAppServices = marcaAppServices;
            _materialAppServices = materialAppServices;
            _produtoAppServices = produtoAppServices;
            _produtoCorAppServices = produtoCorAppServices;
            _produtoFotoAppServices = produtoFotoAppServices;
            _corAppServices = corAppServices;
        }

        [HttpGet]
        public async Task<IActionResult> Produtos(string termo,int pagina = 1)
        {
            IEnumerable<ProdutoViewModel> listaProduto;
            if (termo == null)
            {
                listaProduto = await _produtoAppServices.GetAllAsync(null);
            }
            else
            {
                listaProduto = await _produtoAppServices.FilterBarraPesquisar(termo);
            }

            var listPaged = await listaProduto.ToList().ToPagedListAsync(pagina, 30);

            return View("List", new ProdutoListAdmViewModel(listPaged,termo));
        }

        [HttpGet]
        public async Task<IActionResult> EditProduto(int id)
        {
            string statusModel = null;
            var marcas = await _marcaAppServices.GetAllAsync();
            var materiais = await _materialAppServices.GetAllAsync();

            var listaFotos = await _produtoFotoAppServices.RetornarListaFotoInput(id);
            var cores = await _produtoCorAppServices.GetByProdutoAsync(id, null);
            var listaCores = await _produtoCorAppServices.RetornarListaDeCorDoProduto(id, null);
            var produto = await _produtoAppServices.GetByIdAsync(id);

            var corSelect = await _corAppServices.GetAllAsync();

            if (TempData["Error"] != null)
            {
                statusModel = "Error";
                ModelState.AddModelError(string.Empty, TempData["Error"].ToString());
            }
            else if (TempData["Success"] != null)
            {
                statusModel = "Success";
                ModelState.AddModelError(string.Empty, TempData["Success"].ToString());
            }

            return View("Edit", new ProdutoEditViewModel(marcas,materiais,listaFotos,cores, listaCores, corSelect, produto,id,statusModel));
        }

        public async Task<IActionResult> AlterarEstoque(int id, int quantNumber, int idProduto)
        {
            try
            {
                await _produtoCorAppServices.AlterarEstoqueAsync(id, quantNumber);
                TempData["Success"] = "Alteração processada com sucesso";
            }
            catch (Exception)
            {
                TempData["Error"] = "Ocorreu um erro ao processar a alteração";
            }

            return RedirectToAction("EditProduto", new {id=idProduto});
        }


        [HttpGet]
        public async Task<IActionResult> CreateEtapaBasico()
        {
            var marcas = await _marcaAppServices.GetAllAsync();
            var materiais = await _materialAppServices.GetAllAsync();

            return View("CreateBasico", new ProdutoCreateEtapaBasicaModel(marcas,materiais));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SalvarEtapaProduto([Bind("MarcaModelId,MaterialModelId,Genero,StatusProduto," +
                                                                  "Referencia,Tamanho,Descricao,ValorVenda,BestSeller,Liquidacao,Vitrine")] ProdutoCreateEtapaBasicaModel produtoCreateEtapaBasicaModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                   var (id,status,mensagem) = await  _produtoAppServices.CreateProdutoAsync(produtoCreateEtapaBasicaModel);

                   if (status)
                   {
                       return await CreateCor(id);
                   }

                   produtoCreateEtapaBasicaModel.StatusModel = "Error";
                   ModelState.AddModelError(string.Empty, mensagem);
                }

                produtoCreateEtapaBasicaModel.StatusModel = "Error";
                ModelState.AddModelError(string.Empty, "Preenchimento de dados inválido");
            }
            catch (Exception e)
            {
                produtoCreateEtapaBasicaModel.StatusModel = "Error";
                ModelState.AddModelError(string.Empty, $"Error, type: {e.Message}");
            }

            return View("CreateBasico", produtoCreateEtapaBasicaModel);
        }

        public async Task<IActionResult> UpdateProduto(ProdutoEditViewModel produtoEditViewModel, ProdutoCreateEtapaBasicaModel produtoCreateEtapaBasicaModel,string novaReferencia)
        {
            try
            {
                produtoCreateEtapaBasicaModel.Id = produtoEditViewModel.IdProd;
                novaReferencia = produtoCreateEtapaBasicaModel.Referencia;
                produtoCreateEtapaBasicaModel.Referencia = produtoEditViewModel.Basico.Referencia;
                produtoCreateEtapaBasicaModel.StatusAtivacao = produtoEditViewModel.Basico.StatusAtivacao;

                var (status,mensagem)=await _produtoAppServices.UpdateAsync(produtoCreateEtapaBasicaModel, novaReferencia);

                if (!status)
                {
                    TempData["Error"] = mensagem;
                }
            }
            catch (Exception e)
            {
                TempData["Error"] = $"Error, type: {e.Message}";
            }

            return RedirectToAction("EditProduto", new { id = produtoEditViewModel.IdProd });
        }

        [HttpGet]
        private async Task<IActionResult> CreateCor(int idProduto)
        {
            var coresSelect = await _corAppServices.GetAllAsync();
            return View("CreateCor", new ProdutoCreateEtapaCorModel(idProduto, null,null, coresSelect));
        }

        public async Task<IActionResult> AddCorListaCreate(ProdutoCorInputModel produtoCorInputModel)
        {
            var produtoId = produtoCorInputModel.ProdutoId;
            string statusModel = null;

            var (status, mensagem) = await AdicionarCorAoProduto(produtoCorInputModel, produtoId);

            if (!status)
            {
                statusModel = "Error";
                ModelState.AddModelError(string.Empty, mensagem);
            }

            var listaCores = await _produtoCorAppServices.RetornarListaDeCorDoProduto(produtoId,null);

            var coresSelect = await _corAppServices.GetAllAsync();

            return View("CreateCor", new ProdutoCreateEtapaCorModel(produtoId, listaCores,statusModel,coresSelect));
        }

        public async Task<IActionResult> AddCorListaEdit(ProdutoCorInputModel produtoCorInputModel)
        {
            var produtoId = produtoCorInputModel.ProdutoId;

            var (status, mensagem) = await AdicionarCorAoProduto(produtoCorInputModel, produtoId);

            if (!status)
            {
                TempData["Error"] = mensagem;
            }

            return RedirectToAction("EditProduto", new {id = produtoId});
        }

        private async Task<(bool,string)> AdicionarCorAoProduto(ProdutoCorInputModel produtoCorInputModel, int idProduto)
        {
            if (produtoCorInputModel.Estoque > 0 && produtoCorInputModel.CorId > 0)
            {
                try
                {
                    produtoCorInputModel.ProdutoId = idProduto;
                    var(status,mensagem) = await _produtoCorAppServices.AddListaCor(produtoCorInputModel);
                    return status ? (true, null) : (false, mensagem);
                }
                catch (Exception e)
                {
                    return (false, $"Error, Type: {e.Message}");
                }
            }

            return (false, "Os campos 'Estoque' e 'Cor' são obrigatórios.");
        }

        public async Task<IActionResult> RemoverCorListaCreate(int idCor, int idProd)
        {
            var produtoId = idProd;
            string statusModel = null;

            var (status, mensagem) = await RemoverCorDoProduto(idCor);

            if (!status)
            {
                statusModel = "Error";
                ModelState.AddModelError(string.Empty, mensagem);
            }

            var listaCores = await _produtoCorAppServices.RetornarListaDeCorDoProduto(produtoId,null);
            var coresSelect = await _corAppServices.GetAllAsync();

            return View("CreateCor", new ProdutoCreateEtapaCorModel(produtoId, listaCores,statusModel,coresSelect));
        }

        public async Task<IActionResult> RemoveCorListaEdit(int idCor, int idProd)
        {

            var (status, mensagem) = await RemoverCorDoProduto(idCor);

            if (!status)
            {
                TempData["Error"] = mensagem;
            }

            return RedirectToAction("EditProduto", new { id = idProd });
        }

        private async Task<(bool, string)> RemoverCorDoProduto(int idCor)
        {
            if (idCor > 0)
            {
                try
                {
                    await _produtoCorAppServices.RemoveListaCor(idCor);
                    return (true, null);
                }
                catch (Exception e)
                {
                    return (false, $"Error, Type: {e.Message}");
                }
            }

            return (false, "Erro inesperado ao remover item da lista");
        }

        [HttpGet]
        public async Task<IActionResult> SalvarEtapaCor(int idProd)
        {
            var cores = await _produtoCorAppServices.RetornarListaDeCorDoProduto(idProd,null);

            var produtoCorInputModels = cores.ToList();

            if (produtoCorInputModels.Any())
            {
                return await CreateFotos(idProd);
            }

            const string statusModel = "Error";
            var produtoId = idProd;
            var coresSelect = await _corAppServices.GetAllAsync();

            ModelState.AddModelError(string.Empty, "É necessário pelo menos uma cor para prosseguir com o cadastro da armação");

            return View("CreateCor", new ProdutoCreateEtapaCorModel(produtoId, produtoCorInputModels,statusModel, coresSelect));
        }

        [HttpGet]
        private async Task<IActionResult> CreateFotos(int idProduto, IEnumerable<ProdutoFotoInputModel> fotos = null)
        {
            var cores = await _produtoCorAppServices.GetByProdutoAsync(idProduto,null);

            string statusModel = null;
            var ativacao = "PE";

            if (TempData["ModelError"] != null)
            {
                statusModel = "Error";
                ModelState.AddModelError(string.Empty, TempData["ModelError"].ToString());
            }

            if (TempData["Success"] != null)
            {
                statusModel = "Success";
                ativacao = "AT";
                ModelState.AddModelError(string.Empty, TempData["Success"].ToString());
            }

            return View("CreateFotos", new ProdutoCreateEtapaFotoModel(fotos, cores, idProduto, statusModel,ativacao));

        }

        public async Task<IActionResult> AddFotoListaCreate(IFormFile foto, ProdutoFotoInputModel produtoFotoInputModel)
        {
            var produtoId = produtoFotoInputModel.ProdutoId;

            var (status, mensagem) = await AdicionarFotoAoProduto(produtoFotoInputModel, foto, produtoId);

            if (!status)
            {
                TempData["Error"] = mensagem;
            }

            var listaFotos = await _produtoFotoAppServices.RetornarListaFotoInput(produtoId);

            return await CreateFotos(produtoId, listaFotos);
        }

        public async Task<IActionResult> AddFotoListaEdit(IFormFile foto, ProdutoFotoInputModel produtoFotoInputModel)
        {
            var produtoId = produtoFotoInputModel.ProdutoId;

            var (status, mensagem) = await AdicionarFotoAoProduto(produtoFotoInputModel, foto, produtoId);

            if (!status)
            {
                TempData["Error"] = mensagem;
            }

            return RedirectToAction("EditProduto", new { id = produtoId });
        }

        private async Task<(bool, string)> AdicionarFotoAoProduto(ProdutoFotoInputModel produtoFotoInputModel, IFormFile foto, int idProduto)
        {
            if (produtoFotoInputModel.CorId > 0 && foto != null)
            {
                try
                {
                    produtoFotoInputModel.ProdutoId = idProduto;
                    var(status,mensagem)=await _produtoFotoAppServices.AddFotoLista(foto, produtoFotoInputModel, null);

                    return status ? (true, null) : (false, mensagem);

                }
                catch (Exception e)
                {
                    return (false, $"Error, type: {e.Message}");
                }
            }

            return (false, "Campo cor e foto são obrigatorios!");
        }

        public async Task<IActionResult> RemoverFotoListaCreate(int idFoto, int idProd)
        {
            var produtoId = idProd;

            var (status, mensagem) = await RemoverFotoDoProduto(idFoto);

            if (!status)
            {
                TempData["ModelError"] = mensagem;
            }

            var listaFotos = await _produtoFotoAppServices.RetornarListaFotoInput(produtoId);

            return await CreateFotos(produtoId, listaFotos);
        }

        public async Task<IActionResult> RemoverFotoListaEdit(int idFoto, int idProd)
        {
            var (status, mensagem) = await RemoverFotoDoProduto(idFoto);

            if (!status)
            {
                TempData["Error"] = mensagem;
            }

            return RedirectToAction("EditProduto", new { id = idProd });
        }

        private async Task<(bool, string)> RemoverFotoDoProduto(int idFoto)
        {
            if (idFoto > 0)
            {
                try
                {
                    await _produtoFotoAppServices.RemoveListaFoto(idFoto);
                    return (true, null);
                }
                catch (Exception e)
                {
                    return (false, $"Error, type: {e.Message}");
                }
            }

            return (false, "Erro inesperado ao remover item da lista");
        }

        [HttpGet]
        public async Task<IActionResult> SalvarEtapaFoto(int idProd)
        {
            
            if (idProd > 0)
            {
                var verificacao = await _produtoFotoAppServices.CheckFotoAndPrincipal(idProd);

                if (verificacao)
                {
                    try
                    {
                        await _produtoFotoAppServices.CreateAsync(idProd, null);
                        TempData["Success"] = "Cadastro concluído com sucesso!";
                    }
                    catch (Exception e)
                    {
                        TempData["ModelError"] = $"Erro, type: {e.Message}";
                    }
                }
                else
                {
                    TempData["ModelError"] = "Necessária no mínimo uma foto principal para finalizar o cadastro.";
                }
            }
            else
            {
                TempData["ModelError"] = "Nenhum produto vinculado para realizar o cadastro";
            }

            var listaFotos = await _produtoFotoAppServices.RetornarListaFotoInput(idProd);

            return await CreateFotos(idProd, listaFotos);
        }

        public async Task<IActionResult> AlterarStatusAtivacaoCor(int idCor, int idProd)
        {
            try
            {
                await _produtoCorAppServices.UpdateAsync(idCor);
            }
            catch (Exception e)
            {
                TempData["Error"] = $"Error, Type: {e.Message}";
            }

            return RedirectToAction("EditProduto", new { id = idProd });
        }

        public async Task<IActionResult> AlterarStatusAtivacaoProduto(int idProduto)
        {
            try
            {
                await _produtoAppServices.AlterarStatusAtivacaoProduto(idProduto);
            }
            catch (Exception e)
            {
                TempData["Error"] = $"Error, Type: {e.Message}";
            }

            return RedirectToAction("Produtos");
        }

        public async Task AlterarKitDoProduto(int idCor, string kit)
        {
            try
            {
                await _produtoCorAppServices.AlterarAtivacaoKitNoProduto(idCor, kit);
            }
            catch (Exception e)
            {
                TempData["Error"] = $"Error, Type: {e.Message}";
            }
        }

        public async Task<IActionResult> GetAviseMe(int pagina = 1)
        {
            var listaProduto = await _produtoAppServices.GetAviseMe();

            var listPaged = await listaProduto.ToList().ToPagedListAsync(pagina, 30);

            return View("AviseMe",listPaged);
        }

        public async Task<IActionResult> AtualizarEstoqueMassa(int quantidadeAtuMassa)
        {
            try
            {
                await _produtoAppServices.AtualizarEstoqueMassa(quantidadeAtuMassa);
                TempData["Success"] = "Estoque atualizado com sucesso!";
            }
            catch (Exception e)
            {
                TempData["Error"] = $"Error, Type: {e.Message}";
            }

            return RedirectToAction("Produtos");
        }
    }
}
