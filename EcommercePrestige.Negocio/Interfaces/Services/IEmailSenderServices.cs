using System.Threading.Tasks;

namespace EcommercePrestige.Model.Interfaces.Services
{
    public interface IEmailSenderServices
    {
        Task SendEmailAsync(string email, string subject, string message, byte[] anexo, string nomeArquivo);
    }
}
