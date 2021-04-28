namespace EcommercePrestige.Application.ViewModel
{
    public class ArmacoesViewModel
    {
        public int Armacao { get; set; }
        public int Cor { get; set; }
        public int Quantidade { get; set; }

        public ArmacoesViewModel(int id, int cor, int quantidade)
        {
            Quantidade = quantidade;
            Armacao = id;
            Cor = cor;
        }
    }
}
