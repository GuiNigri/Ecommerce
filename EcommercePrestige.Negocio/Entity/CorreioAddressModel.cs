namespace EcommercePrestige.Model.Entity
{
    public class CorreioAddressModel
    {
        public string Cep { get; private set; }
        public string Rua { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }

        public CorreioAddressModel(string cep, string rua, string bairro, string cidade, string estado)
        {
            Cep = cep;
            Rua = rua;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
        }
    }
}
