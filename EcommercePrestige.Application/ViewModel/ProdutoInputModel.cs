using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EcommercePrestige.Application.ViewModel
{
    public class ProdutoCreateEtapaCorModel : BaseViewModel
    {
        public IEnumerable<ProdutoCorInputModel> Cores { get; set; } 
        public ProdutoCorInputModel  Cor { get; set; }

        public ProdutoCreateEtapaCorModel()
        {
        }

        public ProdutoCreateEtapaCorModel(int idProduto, IEnumerable<ProdutoCorInputModel> cores, string statusModel, IEnumerable<CorViewModel> coresSelect)
        {
            Cor = new ProdutoCorInputModel(idProduto,coresSelect);
            Cores = cores ?? new List<ProdutoCorInputModel>();
            StatusModel = statusModel;
        }

        private static List<SelectListItem> PopulateCorRgb(IEnumerable<CorViewModel> cor)
        {
            return cor.Select(item => new SelectListItem() { Value = item.Id.ToString(), Text = item.Descricao }).ToList();
        }
    }

    public class ProdutoCreateEtapaBasicaModel:BaseViewModel
    {
        public int MarcaModelId { get; set; }
        public List<SelectListItem> SelectMarca { get; set; }
        public int MaterialModelId { get; set; }
        public List<SelectListItem> SelectMaterial { get; set; }
        public string Genero { get; set; }
        public SelectListItem[] SelectGenero { get; set; }
        public string StatusProduto { get; set; }
        public SelectListItem[] SelectStatus { get; set; }
        public string Referencia { get; set; }
        public string Tamanho { get; set; }
        public string Descricao { get; set; }
        public string ValorVenda { get; set; }
        public bool BestSeller { get; set; }
        public bool Liquidacao { get; set; }
        public bool Vitrine { get; set; }
        public string StatusAtivacao { get; set; }

        public ProdutoCreateEtapaBasicaModel()
        {
        }

        public ProdutoCreateEtapaBasicaModel(IEnumerable<MarcaViewModel> marca, IEnumerable<MaterialViewModel> material)
        {
            SelectMaterial = PopulateMaterial(material);
            SelectMarca = PopulateMarca(marca);
            SelectStatus = PopulateStatus();
            SelectGenero = PopulateGenero();
        }

        public ProdutoCreateEtapaBasicaModel(IEnumerable<MarcaViewModel> marca, IEnumerable<MaterialViewModel> material, ProdutoViewModel produtoViewModel)
        {
            SelectMaterial = PopulateMaterial(material);
            SelectMarca = PopulateMarca(marca);
            SelectStatus = PopulateStatus();
            SelectGenero = PopulateGenero();
            MaterialModelId = produtoViewModel.MaterialModelId;
            MarcaModelId = produtoViewModel.MarcaModelId;
            Genero = produtoViewModel.Genero;
            StatusProduto = produtoViewModel.StatusProduto;
            Referencia = produtoViewModel.Referencia;
            Tamanho = produtoViewModel.Tamanho;
            Descricao = produtoViewModel.Descricao;
            ValorVenda = produtoViewModel.ValorVenda;
            BestSeller = produtoViewModel.BestSeller;
            Liquidacao = produtoViewModel.Liquidacao;
            Vitrine = produtoViewModel.Vitrine;
            StatusAtivacao = produtoViewModel.StatusAtivacao;
        }

        private static List<SelectListItem> PopulateMarca(IEnumerable<MarcaViewModel> marca)
        {
            return marca.Select(item => new SelectListItem() { Value = item.Id.ToString(), Text = item.Nome }).ToList();
        }

        private static List<SelectListItem> PopulateMaterial(IEnumerable<MaterialViewModel> material)
        {
            return material.Select(item => new SelectListItem() { Value = item.Id.ToString(), Text = item.Material }).ToList();
        }

        private static SelectListItem[] PopulateGenero()
        {
            var lista = new[]
            {
                new SelectListItem{Value = "Masculino",Text = "Masculino"},
                new SelectListItem{Value = "Feminino",Text = "Feminino"},
                new SelectListItem{Value = "Unissex",Text = "Unissex"}
            };
            return lista;
        }

        private static SelectListItem[] PopulateStatus()
        {
            var lista = new[]
            {
                new SelectListItem{Value = "new",Text = "Lançamento"},
                new SelectListItem{Value = "normal",Text = "Normal"},
                new SelectListItem{Value = "offline",Text = "Descontinuado"}
            };
            return lista;
        }

    }

    public class ProdutoCorInputModel:BaseViewModel
    {
        public int ProdutoId { get; set; }
        public int CorId { get; set; }
        public string DescricaoCor { get; set; }
        public string ImgCor { get; set; }
        public string CodigoInterno { get; set; }
        public int Estoque { get; set; }
        public string CodigoBarras { get; set; }
        public bool Gold { get; set; }
        public bool Silver { get; set; }
        public bool Basic { get; set; }
        public string StatusAtivacao { get; set; }
        public IEnumerable<ProdutoCorInputModel> Cores { get; set; }
        public List<SelectListItem> CoresSelect { get; set; }

        public ProdutoCorInputModel() {}
        public ProdutoCorInputModel(int id, IEnumerable<CorViewModel> corSelect)
        {
            ProdutoId = id;
            CoresSelect = PopulateCorRgb(corSelect);
        }

        private static List<SelectListItem> PopulateCorRgb(IEnumerable<CorViewModel> cor)
        {
            return cor.Select(item => new SelectListItem() { Value = item.Id.ToString(), Text = item.Descricao }).ToList();
        }
    }

    public class ProdutoCreateEtapaFotoModel : BaseViewModel
    {
        public IEnumerable<ProdutoFotoInputModel> Fotos { get; set; }
        public ProdutoFotoInputModel Foto { get; set; }
        public string StatusAtivacao { get; set; }
        

        public ProdutoCreateEtapaFotoModel()
        {
        }

        public ProdutoCreateEtapaFotoModel(IEnumerable<ProdutoFotoInputModel> fotos, IEnumerable<ProdutoCorViewModel> produtoCorViewModels, int idProd, string statusModel, string ativacao)
        {
            Fotos = fotos ?? new List<ProdutoFotoInputModel>();
            Foto = new ProdutoFotoInputModel(idProd,produtoCorViewModels);
            StatusModel = statusModel;
            StatusAtivacao = ativacao;
        }

    }

    public class ProdutoFotoInputModel : BaseViewModel
    {
        public int ProdutoId { get; set; }
        public int CorId { get; set; }
        public string ImgCor { get; set; }
        public string Descricao { get; set; }
        public string Foto { get; set; }
        public bool Principal { get; set; }
        public string StatusAtivacao { get; set; }
        public List<SelectListItem> SelectCor { get; set; }

        public ProdutoFotoInputModel()
        {
        }

        public ProdutoFotoInputModel(int id, IEnumerable<ProdutoCorViewModel> selectCor)
        {
            ProdutoId = id;
            SelectCor = PopulateCor(selectCor);
        }

        private static List<SelectListItem> PopulateCor(IEnumerable<ProdutoCorViewModel> cor)
        {
            return cor.Select(item => new SelectListItem() { Value = item.CorId.ToString(), Text = item.DescricaoCor }).ToList();
        }
    }

    public class ProdutoEditViewModel : BaseViewModel
    {
        public int IdProd { get; set; }
        public ProdutoCreateEtapaBasicaModel Basico { get; set; }
        public ProdutoCreateEtapaCorModel EtapaCor { get; set; }
        public ProdutoCreateEtapaFotoModel EtapaFoto { get; set; }

        public ProdutoEditViewModel()
        {
        }

        public ProdutoEditViewModel(IEnumerable<MarcaViewModel> marca, IEnumerable<MaterialViewModel> material, IEnumerable<ProdutoFotoInputModel> fotos,
            IEnumerable<ProdutoCorViewModel> cores, IEnumerable<ProdutoCorInputModel> coresInput, IEnumerable<CorViewModel> corSelect, ProdutoViewModel produtoViewModel, int idProd,string statusModel)
        {
            IdProd = idProd;
            Basico = new ProdutoCreateEtapaBasicaModel(marca,material,produtoViewModel);
            EtapaCor = new ProdutoCreateEtapaCorModel(idProd, coresInput,null, corSelect);
            EtapaFoto = new ProdutoCreateEtapaFotoModel(fotos,cores, idProd, null,null);
            StatusModel = statusModel;
        }
    }


}
