using System;
using System.Collections.Generic;
using System.Text;

namespace EcommercePrestige.Model.Entity
{
    public class EmpresaApiModel
    {
        public string Status { get; private set; }
        public string Message { get; private set; }
        public string Cnpj { get; private set; }
        public string Nome { get; private set; }
        public IEnumerable<CnaeModel> Atividade_principal { get; private set; }
        public string Logradouro { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Cep { get; private set; }
        public string Bairro { get; private set; }
        public string Municipio { get; private set; }
        public string Uf { get; private set; }
        public string Telefone { get; private set; }

        public void setTelefone(string telefone)
        {
            Telefone = telefone;
        }

        public void setStatus(string status)
        {
            Status = status;
        }

        public void setMessage(string message)
        {
            Message = message;
        }
    }

    public class CnaeModel
    {
        public string Code { get; private set; }
    }
}
