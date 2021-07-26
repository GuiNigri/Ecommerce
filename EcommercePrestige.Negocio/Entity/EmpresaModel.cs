namespace EcommercePrestige.Model.Entity
{
    public class EmpresaModel : BaseModel
    {
        public string UserId { get; private set; }

        public string Cnpj { get; private set; }

        public string RazaoSocial { get; private set; }
        public string Cnae { get; private set; }
        public string Logradouro { get; private set; }
        public int Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Cep { get; private set; }
        public string Bairro { get; private set; }
        public string Municipio { get; private set; }
        public string Uf { get; private set; }
        public string Telefone { get; private set; }
        public string NomeOtica { get; private set; }

        public EmpresaModel(int id,string userId, string cnpj, string razaoSocial, string cnae, string logradouro, int numero, string complemento, string cep, string bairro, string municipio, string uf, string telefone, string nomeOtica)
        {
            Id = id;
            UserId = userId;
            Cnpj = cnpj;
            RazaoSocial = razaoSocial;
            Cnae = cnae;
            Logradouro = logradouro;
            Numero = numero;
            Complemento = complemento;
            Cep = cep;
            Bairro = bairro;
            Municipio = municipio;
            Uf = uf;
            Telefone = telefone;
            NomeOtica = nomeOtica;
        }


    }

    public class CidadesModel : BaseModel
    {
        public string Cidade { get; set; }
    }
}
