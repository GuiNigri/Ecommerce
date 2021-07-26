using System.Threading.Tasks;

namespace EcommercePrestige.Application.AppServices
{
    public interface ISuporteAppServices
    {
        Task SendAutomaticSuporteEmail(string htmlMessage, string email, string subject);

    }
}
