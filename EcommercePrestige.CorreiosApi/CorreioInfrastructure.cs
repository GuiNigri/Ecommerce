using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Correios.NET;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Infrastructure;
using MoreLinq;
using ServiceReferenceCorreios;

namespace EcommercePrestige.CorreiosApi
{
    public class CorreioInfrastructure:ICorreiosInfrastructure
    {
        private readonly Services _correios;

        public CorreioInfrastructure(Services correios)
        {
            _correios = correios;
        }

        public async Task<IEnumerable<TrackingHistoryModel>> PackageTracking(string rastreio)
        {
            await CalcularFrete("70710904", 100,1243.8);
            var package = await _correios.GetPackageTrackingAsync(rastreio);

            return package.TrackingHistory.Select(item => new TrackingHistoryModel(item.Details, item.Status, item.Date, item.Location)).ToList();

        }

        public async Task<CorreioAddressModel> AddressByZipCode(string cep)
        {
            var addressRequest = await _correios.GetAddressesAsync(cep);
            //await Task.WhenAll(addressRequest);

            var retornoEndereco = addressRequest.ToList();

            var verificacaoEndereco = retornoEndereco[0].Street.Contains("-");

            var correioAddressModel = new List<CorreioAddressModel>();

            if (verificacaoEndereco)
            {
                correioAddressModel = retornoEndereco.Select(item =>
                    new CorreioAddressModel(item.ZipCode, item.Street.Substring(0, item.Street.IndexOf(" -", StringComparison.Ordinal)),item.District,item.City,item.State)).ToList();
            }
            else
            {
                correioAddressModel = retornoEndereco.Select(item =>
                    new CorreioAddressModel(item.ZipCode, item.Street, item.District, item.City, item.State)).ToList();
            }


            return correioAddressModel[0];

        }

        public async Task<bool> VerificarSeEntregue(string rastreio)
        {
            var package = await _correios.GetPackageTrackingAsync(rastreio);

            return package.IsDelivered;
        }

        public async Task<IEnumerable<CorreioWebServiceModel>> CalcularFrete(string cepDestino, int quantidadePecas, double valorTotalPedido)
        {
            try
            {
                

                var pacotes = new List<InformacoesPacoteCorreiosModel>();

                var mediaPrecoDosProdutos = valorTotalPedido / quantidadePecas;

                while (quantidadePecas > 0)
                {
                    var pacote = new InformacoesPacoteCorreiosModel();

                    if (quantidadePecas > 55)
                    {
                        pacote.SetPackageData(new CaixaCorreioModel(35, 25, 30, 0.2), 55);

                        quantidadePecas -= 55;

                    }
                    else if (quantidadePecas > 30 && quantidadePecas <= 55)
                    {

                        pacote.SetPackageData(new CaixaCorreioModel(35, 25, 30, 0.2), quantidadePecas);

                        quantidadePecas -= quantidadePecas;

                    }
                    else if (quantidadePecas <= 30)
                    {

                        pacote.SetPackageData(new CaixaCorreioModel(30, 20, 24, 0.170), quantidadePecas);

                        quantidadePecas -= quantidadePecas;
                    }

                    pacotes.Add(pacote);

                }


                //Dados da Empresa se tiver contrato
                const string nCdEmpresa = "";
                const string sDsSenha = "";
                const string sCepOrigem = "20270004";
                const int nCdFormato = 1; //FORMATO DE CAIXA
                const string sCdMaoProrpia = "N";

                var sCepDestino = cepDestino;


                var resultList = new List<CorreioWebServiceModel>();

                //loop
                foreach (var pacote in pacotes)
                {
                    var pesoEnvioCorreio = ((0.02 * pacote.QuantidadeArmacoes) + pacote.Caixa.Peso).ToString();
                    //PESO TOTAL EM KG
                    var nVlPeso = pesoEnvioCorreio;


                    //PARA PAC PREENCHER OS CAMPOS
                    decimal nVlComprimento = pacote.Caixa.Comprimento;
                    decimal nVlAltura = pacote.Caixa.Altura;
                    decimal nVlLargura = pacote.Caixa.Largura;
                    decimal nVlDiametro = 0;


                    var valorPacote = mediaPrecoDosProdutos * pacote.QuantidadeArmacoes;
                    decimal nVlValorDeclarado = Convert.ToInt32(Math.Round(valorPacote));

                    var sCdAvisoRecebimento = "N";

                    var servicesList = new List<string>
                    {
                        "40010",
                        "41106"
                    };


                    foreach (var service in servicesList)
                    {
                        var nCdServico = service;



                        var nomeServico = service == "40010" ? "Sedex" : "Pac";

                        var wsCorreios =
                            new CalcPrecoPrazoWSSoapClient(CalcPrecoPrazoWSSoapClient.EndpointConfiguration.CalcPrecoPrazoWSSoap);

                        var retornoCorreios = await wsCorreios.CalcPrecoPrazoAsync(nCdEmpresa, sDsSenha, nCdServico, sCepOrigem, sCepDestino, nVlPeso, nCdFormato, nVlComprimento, nVlAltura, nVlLargura, nVlDiametro, sCdMaoProrpia, nVlValorDeclarado, sCdAvisoRecebimento);

                        var correiosWsModel = new CorreioWebServiceModel();

                        if (valorTotalPedido > 1500 && nomeServico == "Pac")
                        {
                            correiosWsModel.SetWebServiceData("Proprio", retornoCorreios.Servicos[0].PrazoEntrega, "0,00");

                        }
                        else
                        {
                            correiosWsModel.SetWebServiceData(nomeServico, retornoCorreios.Servicos[0].PrazoEntrega, retornoCorreios.Servicos[0].Valor);

                        }

                        if (correiosWsModel.PrazoEntrega != "0")
                        {
                            resultList.Add(correiosWsModel);
                        }

                    }
                }

                if (resultList.Count == 1 && valorTotalPedido > 1500)
                {
                    resultList[0].SetValor("0,00");
                }


                var resultGrouped = from empresa in resultList.GroupBy(x=>x.Servico)
                    select new CorreioWebServiceModel(empresa.First().Servico, empresa.First().PrazoEntrega,
                        empresa.Sum(a => double.Parse(a.Valor)).ToString("N2"));

                return resultGrouped;


            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}
