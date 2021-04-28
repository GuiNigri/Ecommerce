using System.Threading.Tasks;
using EcommercePrestige.Model.Interfaces.Services;

namespace EcommercePrestige.Application.AppServices.Implementations
{
    public class SuporteAppServices:ISuporteAppServices
    {
        private readonly ISuporteServices _domainService;

        public SuporteAppServices(ISuporteServices domainService)
        {
            _domainService = domainService;
        }


        public async Task SendAutomaticSuporteEmail(string htmlMessage, string email, string subject)
        {
            await _domainService.SendAutomaticSuporteEmail(htmlMessage, email,subject);
        }
    }
}
