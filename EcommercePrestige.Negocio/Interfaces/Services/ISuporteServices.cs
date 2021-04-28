using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;

namespace EcommercePrestige.Model.Interfaces.Services
{
    public interface ISuporteServices:IBaseServices<SuporteModel>
    {
        Task SendAutomaticSuporteEmail(string htmlMessage, string email, string subject);
    }
}
