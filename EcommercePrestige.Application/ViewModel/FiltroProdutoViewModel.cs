namespace EcommercePrestige.Application.ViewModel
{
    public class FiltroProdutoViewModel : BaseViewModel
    {
        public int  CorOption { get; set; }
        public string ImgCor { get; set; }
        public string MarcaOption { get; set; }
        public string GeneroOption { get; set; }
        public string MaterialOption { get; set; }
        public string OrderType { get; set; }
        public string Category { get; set; }
        public string Termo { get; set; }

        public FiltroProdutoViewModel()
        {
        }
        public FiltroProdutoViewModel(int cor, string marca, string genero, string orderOption, string material, string category, string imgCor, string termo)
        {
            CorOption = cor;
            MarcaOption = marca;
            GeneroOption = genero;
            OrderType = orderOption;
            Category = category;
            MaterialOption = material;
            ImgCor = imgCor;
            Termo = termo;
        }

        public FiltroProdutoViewModel(string category)
        {
            Category = category;
        }

        public FiltroProdutoViewModel(string marca, string genero, string category,string material)
        {
            MarcaOption = marca;
            GeneroOption = genero;
            Category = category;
            MaterialOption = material;
        }

    }
}
