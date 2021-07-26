using System.Collections.Generic;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;

namespace EcommercePrestige.Services
{
    public class UsuarioServices:BaseServices<UsuarioModel>,IUsuarioServices
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ISuporteServices _suporteServices;

        public UsuarioServices(IUsuarioRepository usuarioRepository, ISuporteServices suporteServices) : base(usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
            _suporteServices = suporteServices;
        }

        public async Task<UsuarioModel> GetByUserIdAsync(string userId)
        {
            return await _usuarioRepository.GetByUserIdAsync(userId);
        }
        public async Task<IEnumerable<UsuarioModel>> Filter(string termo, bool pendente, bool bloqueado)
        {
            return await _usuarioRepository.Filter(termo,pendente,bloqueado);
        }

        public async Task<IEnumerable<UsuarioModel>> GetPendentesAsync()
        {
            return await _usuarioRepository.GetPendentesAsync();
        }

        public async Task<IEnumerable<UsuarioModel>> GetBloqueadosAsync()
        {
            return await _usuarioRepository.GetBloqueadosAsync();
        }
        public async Task SetarStatusCadastroAsync(string status, string userId, string email)
        {
            var usuarioModel = await _usuarioRepository.GetByUserIdAsync(userId);

            var statusBloqueioAutomatico = status switch
            {
                "aprovado" => false,
                "reprovado" => true,
                _ => usuarioModel.BloqueioAutomatico
            };

            usuarioModel.SetBloqueioAutomatico(statusBloqueioAutomatico);

            usuarioModel.SetVerificado(true);

            await _usuarioRepository.UpdateAsync(usuarioModel);

            string htmlMessage;

            if (status == "aprovado")
            {
                htmlMessage =
                    $"Olá, {usuarioModel.NomeCompleto}<br>" +
                    $"Seu cadastro foi <strong>APROVADO</strong> por nossa equipe<br>" +
                    $"Seja muito bem vindo a Prestige do Brasil!<br><br>" +
                    $"Boas Vendas!";
            }
            else
            {
                htmlMessage =
                    $"Olá, {usuarioModel.NomeCompleto}<br>" +
                    $"Seu cadastro foi analisado pela nossa equipe e teve o status alterado para: <strong>{status.ToUpper()}</strong><br><br>" +
                    $"Caso tenha restado alguma duvida entre em contato conosco pelos nossos canais de atendimento.<br><br><br>" +
                    $"Equipe de relacionamento com o cliente<br>" +
                    $"Prestige do Brasil";
            }



            await _suporteServices.SendAutomaticSuporteEmail(htmlMessage, email, "Análise de cadastro");
        }

        public async Task CreateAsync(UsuarioModel usuarioModel, string email, string callBackUrl)
        {
            await _usuarioRepository.CreateAsync(usuarioModel);

            var htmlMessage =
                $"Olá, {usuarioModel.NomeCompleto}<br>" +
                $"Seu cadastro foi efetuado pela nossa equipe, para confirmar seu email <a href='{HtmlEncoder.Default.Encode(callBackUrl)}'>clique neste link</a>.<br><br>" +
                $"Para definir sua senha acesse a area de login do site e clique em esqueci minha senha, encaminharemos um email contendo o link de criação de senha!<br><br><br>" +
                $"Desejamos boas vindas!<br>" +
                $"Equipe Prestige do Brasil";

            await _suporteServices.SendAutomaticSuporteEmail(htmlMessage, email, "Cadastro realizado com sucesso");
        }

    }
}
