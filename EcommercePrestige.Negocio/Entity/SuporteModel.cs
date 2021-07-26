using System;

namespace EcommercePrestige.Model.Entity
{
    public class SuporteModel:BaseModel
    {
        public string Nome { get; private set; }
        public string Email { get; private set; }
        public string DocumentoUsuario { get; private set; }
        public bool Cpf { get; private set; }
        public bool Cnpj { get; private set; }
        public string Assunto { get; private set; }
        public string Mensagem { get; private set; }
        public int Status { get; private set; }
        public DateTime Data { get; private set; }

        public SuporteModel()
        {
            
        }
        public SuporteModel(string documento, int id)
        {
            Id = id;
            DocumentoUsuario = documento;
        }

    }
}
