using System;
using System.Collections.Generic;

namespace EcommercePrestige.Model.Entity
{
    public class PedidoModel:BaseModel
    {
        public int UsuarioModelId { get; private set; }
        public virtual UsuarioModel UsuarioModel { get; private set; }
        public string Rastreio { get; private set; }
        public string FormaDePagamento { get; private set; }
        public string TipoDeEnvio { get; private set; }
        public string Parcelas { get; private set; }
        public double Subtotal { get; private set; }
        public double Desconto { get; private set; }
        public double ValorTotal { get; private set; }
        public double Frete { get; private set; }
        public string ServicoEnvio { get; private set; }
        public string Cep { get; private set; }
        public string Rua { get; private set; }
        public string Numero { get; private set; }
        public string Complemento { get; private set; }
        public string Bairro { get; private set; }
        public string Cidade { get; private set; }
        public string Estado { get; private set; }
        public DateTime DataPedido { get; private set; }
        public string Obs { get; private set; }
        public int Status { get; private set; }
        public string TransactionStatus { get; private set; }
        public string Tid { get; private set; }
        public string AuthorizationCode { get; private set; }
        public string Gateway { get; private set; }
        public virtual ICollection<PedidoProdutosModel> ListaDeProdutos { get; private set; }

        public PedidoModel(int usuarioModelId, string formaDePagamento, string parcelas, double subtotal, double desconto, double valorTotal, double frete, string servicoEnvio, string cep, string rua, string numero, string complemento, string bairro, string cidade, string estado, DateTime dataPedido, string obs, int status)
        {
            UsuarioModelId = usuarioModelId;
            FormaDePagamento = formaDePagamento;
            Parcelas = parcelas;
            Subtotal = subtotal;
            Desconto = desconto;
            ValorTotal = valorTotal;
            Frete = frete;
            ServicoEnvio = servicoEnvio;
            Cep = cep;
            Rua = rua;
            Numero = numero;
            Complemento = complemento;
            Bairro = bairro;
            Cidade = cidade;
            Estado = estado;
            DataPedido = dataPedido;
            Obs = obs;
            Status = status;
        }

        public void SetStatus(int status)
        {
            Status = status;
        }

        public void SetRastreio(string rastreio)
        {
            Rastreio = rastreio;
        }

        public void SetTipoDeEnvio(string tipoDeEnvio)
        {
            TipoDeEnvio = tipoDeEnvio;
        }

        public void SetSubTotal(double subTotal)
        {
            Subtotal = subTotal;
        }
        public void SetValorTotal(double valorTotal)
        {
            ValorTotal = valorTotal;
        }
        public void SetTransactionStatus(string transactionStatus)
        {
            TransactionStatus = transactionStatus;
        }
        public void SetTid(string tid)
        {
            Tid = tid;
        }
        public void SetAuthorizationCode(string authorizationCode)
        {
            AuthorizationCode = authorizationCode;
        }
        public void SetGateway(string gateway)
        {
            Gateway = gateway;
        }
    }
}
