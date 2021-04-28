using System;
using System.Collections.Generic;
using System.Text;

namespace EcommercePrestige.Model.Entity
{
    public class CorreioWebServiceModel
    {
        public string Servico { get; private set; }
        public string PrazoEntrega { get; private set; }
        public string Valor { get; private set; }

        public CorreioWebServiceModel() { }

        public CorreioWebServiceModel(string servico, string prazoEntrega, string valor)
        {
            Servico = servico;
            PrazoEntrega = prazoEntrega;
            Valor = valor;
        }

        public void SetWebServiceData(string servico, string prazoEntrega, string valor)
        {
            Servico = servico;
            PrazoEntrega = prazoEntrega;
            Valor = valor;
        }

        public void SetValor(string valor)
        {
            Valor = valor;
        }
    }

    public class CaixaCorreioModel
    {
        public int Largura { get; private set; }
        public int Comprimento { get; private set; }
        public int Altura { get; private set; }
        public double Peso { get; private set; }

        public CaixaCorreioModel() { }

        public CaixaCorreioModel(int comprimento, int largura, int altura, double peso)
        {
            Largura = largura;
            Altura = altura;
            Comprimento = comprimento;
            Peso = peso;
        }
    }

    public class InformacoesPacoteCorreiosModel
    {
        public CaixaCorreioModel Caixa { get; private set; }
        public int QuantidadeArmacoes { get; private set; }
        public IEnumerable<ProdutoModel> Produto { get; private set; }

        public void SetPackageData(CaixaCorreioModel caixa, int quantidadeArmacoes)
        {
            this.Caixa = caixa;
            this.QuantidadeArmacoes = quantidadeArmacoes;

        }
    }
}
