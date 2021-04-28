using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using Newtonsoft.Json;

namespace EcommercePrestige.ConsultaReceitaApi
{
    public class ConsultaReceitaAwsApi:IConsultaCnpjAwsApi
    {
        public async Task<EmpresaApiModel> consultarCNPJ(string cnpj)
        {
            var requisicaoWeb = WebRequest.CreateHttp($"https://www.receitaws.com.br/v1/cnpj/{cnpj}");
            requisicaoWeb.Method = "GET";
            requisicaoWeb.UserAgent = "RequisicaoCNPJ";

            var  empresaModel = new EmpresaApiModel();

            try
            {
                using var resposta = requisicaoWeb.GetResponse();
                var streamDados = resposta.GetResponseStream();
                var reader = new StreamReader(streamDados);
                object objResponse = reader.ReadToEnd();
                empresaModel = JsonConvert.DeserializeObject<EmpresaApiModel>(objResponse.ToString());

                empresaModel.setTelefone(empresaModel.Telefone.Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", ""));

                streamDados.Close();
                resposta.Close();
            }
            catch (Exception ex)
            {
                empresaModel.setStatus("ERROR");

                empresaModel.setMessage(ex.Message.Contains("(429) Too Many Requests.") ? "Sistema de consulta da receita federal ocupado no momento, tente novamente!" : ex.Message);
            }

            return empresaModel;
        }
    }
}
