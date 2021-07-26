using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using YamlDotNet.Serialization;

namespace EcommercePrestige.NfeEBoleto
{
    public class NfeInfrastructure
    {

        public NfeInfrastructure()
        {
        }

        static string focusToken = "token_recebido_do_suporte";
        static string ambienteFocus = "homologacao"; //homologacao ou producao
        
        
        static string autorizarNFe = "http://" + ambienteFocus + ".acrasnfe.acras.com.br/nfe2/autorizar?token=" + focusToken + "&ref=";
        static string consultarStatusNFe = "http://" + ambienteFocus + ".acrasnfe.acras.com.br/nfe2/consultar?token=" + focusToken + "&ref=";

        public async Task GerarNfe(int referencia)
        {
            //novo url da API
            var url = "https://homologacao.focusnfe.com.br/v2/nfe?ref=" + referencia;

            var dadosNFe = new
            {
                //Dados NFe
                natureza_operacao = "Venda de mercadoria",
                forma_pagamento = "0",
                data_emissao = DateTime.Now.ToString("yyyy-MM-dd"),
                tipo_documento = "1",
                finalidade_emissao = "1",
                //Dados emitente
                cnpj_emitente = "SEU_CNPJ",
                logradouro_emitente = "RUA COELHO LISBOA",
                numero_emitente = "501",
                bairro_emitente = "TATUAPÉ",
                municipio_emitente = "SÃO PAULO",
                uf_emitente = "SP",
                cep_emitente = "03323040",
                telefone_emitente = "11999999999",
                inscricao_estadual_emitente = "SUA_INSCRICAO_ESTADUAL",
                regime_tributario_emitente = "1",
                //Dados destinatário
                cnpj_destinatario = "CNPJ _DESTINATARIO",
                inscricao_estadual_destinatario = "INSCRICAO_ESTADUAL_DESTINATARIO",
                nome_destinatario = "NF-E EMITIDA EM AMBIENTE DE HOMOLOGACAO - SEM VALOR FISCAL",
                logradouro_destinatario = "AV PAULISTA",
                numero_destinatario = "1000",
                bairro_destinatario = "1159",
                municipio_destinatario = "SÃO PAULO",
                uf_destinatario = "SP",
                cep_destinatario = "01310100",
                pais_destinatario = "BRASIL",
                telefone_destinatario = "11988888888",
                email_destinatario = "email@email.com.br", //O sistema FocusNFe enviará a nota fiscal para o e-mail passado aqui Totais
                indicador_inscricao_estadual_destinatario = "1",

                icms_base_calculo = "0.00",
                icms_valor_total = "0.00",
                icms_base_calculo_st = "0.00",
                icms_valor_total_st = "0.00",
                valor_produtos = "100.10",
                valor_frete = "0.00",
                valor_seguro = "0.00",
                valor_desconto = "0.00",
                valor_total_ii = "0.00",
                valor_ipi = "0.00",
                valor_pis = "0.00",
                valor_cofins = "0.00",
                valor_outras_despesas = "0.00",
                valor_total = "100.10",
                modalidade_frete = "1",
                informacoes_adicionais_contribuinte = "DOCUMENTO EMITIDO POR ME OU EPP OPTANTE PELO SIMPLES    NACIONAL",
                items = new List<object> { }
            };

            dadosNFe.items.Add(new
            {
                //Informações do produto
                numero_item = "1",
                codigo_produto = "19999/A",
                descricao = "Placa de homenagem 14x10cm",
                codigo_ncm = "73269090",
                cfop = "5102",
                unidade_comercial = "PÇ",
                quantidade_comercial = "1",
                valor_unitario_comercial = "100.10",
                unidade_tributavel = "PÇ",
                quantidade_tributavel = "1",
                valor_unitario_tributavel = "100.10",
                valor_bruto = "100.10",
                inclui_no_total = "1",
                //Impostos
                icms_origem = "0",
                icms_situacao_tributaria = "101",
                icms_aliquota = "1.25",
                icms_valor = Math.Round((1.25 / 100) * 100.10, 2).ToString("0.00", CultureInfo.InvariantCulture),
                pis_situacao_tributaria = "07",
                cofins_situacao_tributaria = "07"
            });

            var sw = new StringWriter();
            //A classe Serializer é Da biblioteca YamlDotNet, ela escreve o YAML na
            //StringWriter passada no método Serialize.
            var serializer = new Serializer();
            serializer.Serialize(sw, dadosNFe);

            //A API precisa receber as informações com codificação UTF8.
            //Convertemos a string para byte[] para podermos enviar com o método
            //UploadData da classe WebClient;
            var bytes = System.Text.Encoding.UTF8.GetBytes(sw.ToString());

            using var webClient = new WebClient();
            //Envia as informações para a URL de autorização da API
            webClient.UploadData(autorizarNFe + referencia, "POST", bytes);
        }
    }
}
