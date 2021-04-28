using System.Collections.Generic;
using System.Threading.Tasks;
using EcommercePrestige.Application.ViewModel;

namespace EcommercePrestige.Application.AppServices
{
    public interface ISuporteAppServices
    {
        Task SendAutomaticSuporteEmail(string htmlMessage, string email, string subject);

    }
}
