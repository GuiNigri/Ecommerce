using System;
using System.Collections.Generic;
using System.Text;

namespace EcommercePrestige.Model.Entity
{
    public class EmpresaApiModel
    {
        public string Status { get; set; }
        public string Message { get; set; }
        public string Cnpj { get; set; }
        public string Nome { get; set; }
        public IEnumerable<CnaeModel> Atividade_principal { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Cep { get; set; }
        public string Bairro { get; set; }
        public string Municipio { get; set; }
        public string Uf { get; set; }
        public string Telefone { get; set; }

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
        public string Code { get; set; }
    }
}
