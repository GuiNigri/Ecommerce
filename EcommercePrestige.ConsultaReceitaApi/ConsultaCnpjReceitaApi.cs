using EcommercePrestige.Model.Entity;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using EcommercePrestige.Model.Interfaces.Repositories;

namespace EcommercePrestige.ConsultaReceitaApi
{


    public class ConsultaCnpjReceitaApi : IConsultaCnpjReceitaApi
    {
        private static readonly CookieContainer Cookies = new CookieContainer();
        private const string UrlBaseReceitaFederal = "http://servicos.receita.fazenda.gov.br/Servicos/cnpjreva/";
        private const string PaginaValidacao = "valida.asp";
        private const string PaginaPrincipal = "Cnpjreva_Solicitacao.asp";
        private const string PaginaCaptcha = "captcha/gerarCaptcha.asp";

        //metodo para capturar a imagem do site

        private string _mensagem;
        private byte[] _imgBytes;
        private EmpresaModel _empresa;

        // PARAMETRO RECEBE O PICTUREBOX INFORMADO NO FORMULARIO. O BITMAP É CRIADO NO PROPRIO MÉTODO
        // RETORNA BOLEANO IGUAL METODO CONSULTA PARA FICAR PADRAO
        public (bool, string) GetCaptcha()
        {
            string htmlResult; // PARA TER UM RETORNO VAZIO CASO OCORRA ERRO
            using (var wc = new CookieAwareWebClient())
            {
                try
                {
                    wc.SetCookieContainer(Cookies);
                    wc.Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; Synapse)";
                    wc.Headers[HttpRequestHeader.KeepAlive] = "300";
                    htmlResult =
                        wc.DownloadString(UrlBaseReceitaFederal +
                                          PaginaPrincipal); // COLOQUEI TRATAMENTO DE ERRO, O SERVIÇO DA RECEITA FORA DO AR GERA ERRO NESSA LINHA
                }
                catch (Exception)
                {
                    _mensagem = "Erro na consulta: Não foi possível carregar a imagem de validação." +
                                "\nServiço da Receita Federal fora do ar ou bloqueado. Tente novamente mais tarde"; // RETORNA MENSAGEM IGUAL O METODO CONSULTA

                    return (false, _mensagem);
                }
            }

            if (htmlResult.Length > 0)
            {
                var wc2 = new CookieAwareWebClient();
                wc2.SetCookieContainer(Cookies);
                wc2.Headers[HttpRequestHeader.UserAgent] = "Mozilla/4.0 (compatible; Synapse)";
                wc2.Headers[HttpRequestHeader.KeepAlive] = "300";
                var data = wc2.DownloadData(UrlBaseReceitaFederal + PaginaCaptcha);

                if (data != null)
                {
                    _imgBytes = data;
                    return (true, "");
                }

                _mensagem =
                    "Erro na consulta: Não foi possível carregar a imagem de validação. Tente novamente mais tarde"; // RETORNA MENSAGEM IGUAL O METODO CONSULTA

                return (false, _mensagem);
            }

            return (false, _mensagem);

        }

        public string RetornarMensagem()
        {
            return _mensagem;
        }

        public byte[] RetornarImgCaptcha()
        {
            return _imgBytes;
        }

        public EmpresaModel RetornarEmpresa()
        {
            return _empresa;
        }

        // retorna true se conseguiu efetuar a consulta 
        // os dados da consulta vão ser armazenados na variavel paginaHTML

        public (bool, string) ConsultaDados(string aCnpj, string aCaptcha)
        {
            aCnpj = aCnpj.Trim(); // EVITAR QUE QUALQUER ESPAÇO ADICIONAL ATRAPALHE
            aCaptcha = aCaptcha.Trim();

            // VALIDACAO MÍNIMA - EVITAR CONSULTA DESNECESSÁRIA AO SITE
            var validacao = ValidaCampos(aCnpj, aCaptcha);

            if (validacao.Item1 == false)
                return (false, validacao.Item2);

            var resp = ObterDados(aCnpj, aCaptcha);

            if (resp.Contains("Verifique se o mesmo foi digitado corretamente"))
            {
                _mensagem = "Erro na consulta: O número do CNPJ não foi digitado corretamente";
                return (false, _mensagem);
            }

            if (resp.Contains("Erro na Consulta"))
            {
                _mensagem += "Erro na consulta: Os caracteres não conferem com a imagem";
                return (false, _mensagem);
            }

            //_empresa = resp.Length > 0 ? MontarDadosEmpresa(aCnpj, resp) : null;

            return (true, null);

        }

        public EmpresaModel MontarDadosEmpresa(string cnpj, string resp)
        {
        
            //var empresa = new EmpresaModel
            //{
            //    Cnpj = RecuperaColunaValor(resp, ColunaEmpresaModel.NumeroDaInscricao),
            //    RazaoSocial = RecuperaColunaValor(resp, ColunaEmpresaModel.RazaoSocial),
            //    Logradouro = RecuperaColunaValor(resp, ColunaEmpresaModel.EnderecoLogradouro),
            //    Numero = Convert.ToInt32(RecuperaColunaValor(resp, ColunaEmpresaModel.EnderecoNumero)),
            //    Bairro = RecuperaColunaValor(resp, ColunaEmpresaModel.EnderecoBairro),
            //    Cep = RecuperaColunaValor(resp, ColunaEmpresaModel.EnderecoCEP),
            //    Cnae = RecuperaColunaValor(resp, ColunaEmpresaModel.AtividadeEconomicaPrimaria),
            //    Municipio = RecuperaColunaValor(resp, ColunaEmpresaModel.EnderecoCidade),
            //    Uf = RecuperaColunaValor(resp, ColunaEmpresaModel.EnderecoEstado)
            //};
        
        
        
            return null;
        }

        public string ObterDados(string aCnpj, string aCaptcha)
        {
            var request = (HttpWebRequest) WebRequest.Create(UrlBaseReceitaFederal + PaginaValidacao);
            request.ProtocolVersion = HttpVersion.Version10;
            request.CookieContainer = Cookies;
            request.Method = "POST";

            string postData = "";
            postData = postData + "origem=comprovante&";
            postData = postData + "cnpj=" + new Regex(@"[^\d]").Replace(aCnpj, string.Empty) + "&";
            postData = postData + "txtTexto_captcha_serpro_gov_br=" + aCaptcha + "&";
            postData = postData + "submit1=Consultar&";
            postData = postData + "search_type=cnpj";

            byte[] byteArray = Encoding.UTF8.GetBytes(postData);
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = byteArray.Length;

            try
            {
                Stream dataStream = request.GetRequestStream(); // PODE OCORRER ERRO AO TENTAR CONECTAR
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();

                WebResponse
                    response = request
                        .GetResponse(); // COLOCADO DENTRO DE TRY CATCH - SE O SERVIÇO FICAR FORA DO AR, DA ERRO NESSA LINHA
                StreamReader stHtml =
                    new StreamReader(response.GetResponseStream(), Encoding.GetEncoding("ISO-8859-1"));

                return stHtml.ReadToEnd();
            }
            catch (Exception)
            {
                _mensagem = "Erro na consulta: Não foi possível consultar o CNPJ." +
                            "\nServiço da Receita Federal fora do ar ou bloqueado. Tente novamente mais tarde"; // SE O SERVIÇO TIVER FORA, CAI AQUI

                return _mensagem;
            }
        }

        public (bool, string) ValidaCampos(string aCnpj, string aCaptcha)
        {
            // VALIDAÇÃO MÍNIMA BASICA.
            // EVITA CONSULTA DESNECESSÁRIA NO SITE DA RECEITA (MUITAS CONSULTAS PODEM LEVAR O BLOQUEIO DO IP)
            var erro = "";

            if (IsCnpj(aCnpj) == false)
                erro += "Erro na consulta: CNPJ não informado corretamente\n";

            if (aCaptcha.Length != 6) // SEMPRE 6 CARACTERES
                erro += "Erro na consulta: Caracteres não informados corretamente\n";

            if (erro.Length <= 0) return (true, null);
            _mensagem = erro;
            return (false, erro);

        }

        //VALIDACAO DE CNPJ
        private static bool IsCnpj(string cnpj)
        {
            var multiplicador1 = new[] {5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2};
            var multiplicador2 = new[] {6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2};

            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Replace(" ", "");

            if (cnpj.Length != 14)
            {
                return false;
            }
            else
            {
                var tempCnpj = cnpj.Substring(0, 12);

                var soma = 0;
                for (var i = 0; i < 12; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

                var resto = (soma % 11);
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                var digito = resto.ToString();
                tempCnpj += digito;

                soma = 0;
                for (var i = 0; i < 13; i++)
                    soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

                resto = (soma % 11);
                if (resto < 2)
                    resto = 0;
                else
                    resto = 11 - resto;

                digito += resto.ToString();
                return cnpj.EndsWith(digito);
            }
        }

        private static string RecuperaColunaValor(string pattern, ColunaEmpresaModel col)
        {
            var s = pattern.Replace("\n", "").Replace("\t", "").Replace("\r", "");

            switch (col)
            {
                case ColunaEmpresaModel.RazaoSocial:
                {
                    s = StringEntreString(s, "<!-- Início Linha NOME EMPRESARIAL -->",
                        "<!-- Fim Linha NOME EMPRESARIAL -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.NomeFantasia:
                {
                    s = StringEntreString(s, "<!-- Início Linha ESTABELECIMENTO -->",
                        "<!-- Fim Linha ESTABELECIMENTO -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.NaturezaJuridica:
                {
                    s = StringEntreString(s, "<!-- Início Linha NATUREZA JURÍDICA -->",
                        "<!-- Fim Linha NATUREZA JURÍDICA -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.AtividadeEconomicaPrimaria:
                {
                    s = StringEntreString(s, "<!-- Início Linha ATIVIDADE ECONOMICA -->",
                        "<!-- Fim Linha ATIVIDADE ECONOMICA -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.AtividadeEconomicaSecundaria:
                {
                    s = StringEntreString(s, "<!-- Início Linha ATIVIDADE ECONOMICA SECUNDARIA-->",
                        "<!-- Fim Linha ATIVIDADE ECONOMICA SECUNDARIA -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.NumeroDaInscricao:
                {
                    s = StringEntreString(s, "<!-- Início Linha NÚMERO DE INSCRIÇÃO -->",
                        "<!-- Fim Linha NÚMERO DE INSCRIÇÃO -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.MatrizFilial:
                {
                    s = StringEntreString(s, "<!-- Início Linha NÚMERO DE INSCRIÇÃO -->",
                        "<!-- Fim Linha NÚMERO DE INSCRIÇÃO -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringSaltaString(s, "</b>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.EnderecoLogradouro:
                {
                    s = StringEntreString(s, "<!-- Início Linha LOGRADOURO -->", "<!-- Fim Linha LOGRADOURO -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.EnderecoNumero:
                {
                    s = StringEntreString(s, "<!-- Início Linha LOGRADOURO -->", "<!-- Fim Linha LOGRADOURO -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringSaltaString(s, "</b>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.EnderecoComplemento:
                {
                    s = StringEntreString(s, "<!-- Início Linha LOGRADOURO -->", "<!-- Fim Linha LOGRADOURO -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringSaltaString(s, "</b>");
                    s = StringSaltaString(s, "</b>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.EnderecoCEP:
                {
                    s = StringEntreString(s, "<!-- Início Linha CEP -->", "<!-- Fim Linha CEP -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.EnderecoBairro:
                {
                    s = StringEntreString(s, "<!-- Início Linha CEP -->", "<!-- Fim Linha CEP -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringSaltaString(s, "</b>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.EnderecoCidade:
                {
                    s = StringEntreString(s, "<!-- Início Linha CEP -->", "<!-- Fim Linha CEP -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringSaltaString(s, "</b>");
                    s = StringSaltaString(s, "</b>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.EnderecoEstado:
                {
                    s = StringEntreString(s, "<!-- Início Linha CEP -->", "<!-- Fim Linha CEP -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringSaltaString(s, "</b>");
                    s = StringSaltaString(s, "</b>");
                    s = StringSaltaString(s, "</b>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.SituacaoCadastral:
                {
                    s = StringEntreString(s, "<!-- Início Linha SITUAÇÃO CADASTRAL -->",
                        "<!-- Fim Linha SITUACAO CADASTRAL -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.DataSituacaoCadastral:
                {
                    s = StringEntreString(s, "<!-- Início Linha SITUAÇÃO CADASTRAL -->",
                        "<!-- Fim Linha SITUACAO CADASTRAL -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringSaltaString(s, "</b>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }
                case ColunaEmpresaModel.MotivoSituacaoCadastral:
                {
                    s = StringEntreString(s, "<!-- Início Linha MOTIVO DE SITUAÇÃO CADASTRAL -->",
                        "<!-- Fim Linha MOTIVO DE SITUAÇÃO CADASTRAL -->");
                    s = StringEntreString(s, "<tr>", "</tr>");
                    s = StringEntreString(s, "<b>", "</b>");
                    return s.Trim();
                }

                default:
                {
                    return s;
                }
            }
        }

        private static string StringEntreString(string str, string strInicio, string strFinal)
        {
            var ini = str.IndexOf(strInicio, StringComparison.Ordinal);
            var fim = str.IndexOf(strFinal, StringComparison.Ordinal);

            if (ini > 0)
                ini += strInicio.Length;

            if (fim > 0)
                fim += strFinal.Length;

            var diff = ((fim - ini) - strFinal.Length);
            if ((fim > ini) && (diff > 0))
                return str.Substring(ini, diff);
            else
                return string.Empty;
        }

        private static string StringSaltaString(string str, string strInicio)
        {
            var ini = str.IndexOf(strInicio, StringComparison.Ordinal);

            if (ini > 0)
            {
                ini += strInicio.Length;
                return str.Substring(ini);
            }
            
                return str;
        }

    }
}
