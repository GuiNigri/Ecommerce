using System.Collections.Generic;
using X.PagedList;


namespace EcommercePrestige.Application.ViewModel
{
    public class ProdutoIndexViewModel:BaseViewModel
    {
        public IPagedList<ProdutoViewModel> ListProdutos { get; set; }

        public IEnumerable<ProdutoCorViewModel> ListCor { get; set; }

        public IEnumerable<MarcaViewModel> ListMarca { get; set; }

        public IEnumerable<MaterialViewModel> ListMaterial { get; set; }

        public FiltroProdutoViewModel Filtro { get; set; }

        


        public ProdutoIndexViewModel(IPagedList<ProdutoViewModel> listProdutos, IEnumerable<ProdutoCorViewModel> listCor,
            IEnumerable<MarcaViewModel> listMarca, IEnumerable<MaterialViewModel> listMaterial, FiltroProdutoViewModel filtro, string statusModel)
        {
            ListProdutos = listProdutos;
            ListCor = listCor;
            ListMarca = listMarca;
            Filtro = filtro;
            ListMaterial = listMaterial;
            StatusModel = statusModel;
        }

    }

}
