using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Application.ViewModel
{
    public class ProdutoViewModel : BaseViewModel
    {
        public string StatusProduto { get; set; }
        public int MarcaModelId { get; set; }
        public string MarcaModel { get; set; }
        public int MaterialModelId { get; set; }
        public string MaterialModel { get; set; }
        public string Referencia { get; set; }
        public string Tamanho { get; set; }
        public int Estoque { get; set; }
        public string Descricao { get; set; }
        public string ValorVenda { get; set; }
        public string UriFoto { get; set; }
        public string Genero { get; set; }
        public string StatusAtivacao { get; set; }
        public bool BestSeller { get; set; }
        public bool Liquidacao { get; set; }
        public bool Vitrine { get; set; }

        public ProdutoViewModel()
        {
        }
        public ProdutoViewModel(string uriFoto, ProdutoModel model)
        {
            Descricao = model.Descricao;
            Id = model.Id;
            MarcaModelId = model.MarcaModelId;
            MaterialModelId = model.MaterialModelId;
            Referencia = model.Referencia;
            Tamanho = model.Tamanho;
            ValorVenda = model.ValorVenda.ToString("C");
            MarcaModel = model.MarcaModel.Nome;
            MaterialModel = model.MaterialModel.Material;
            StatusProduto = model.StatusProduto;
            UriFoto = uriFoto;
            Genero = model.Genero;
            StatusAtivacao = model.StatusAtivacao;
            BestSeller = model.BestSeller;
            Liquidacao = model.Liquidacao;
            Vitrine = model.Vitrine;

        }

    }
}
