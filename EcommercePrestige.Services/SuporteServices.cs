using System;
using System.Threading.Tasks;
using EcommercePrestige.Model.Entity;
using EcommercePrestige.Model.Interfaces.Infrastructure;
using EcommercePrestige.Model.Interfaces.Repositories;
using EcommercePrestige.Model.Interfaces.Services;

namespace EcommercePrestige.Services
{
    public class SuporteServices:BaseServices<SuporteModel>,ISuporteServices
    {
        private readonly ISuporteRepository _baseRepository;
        private readonly IEmailSenderServices _emailSender;

        public SuporteServices(ISuporteRepository baseRepository, IEmailSenderServices emailSender) : base(baseRepository)
        {
            _baseRepository = baseRepository;
            _emailSender = emailSender;
        }


        public async Task SendAutomaticSuporteEmail(string htmlMessage, string email, string subject)
        {
            await _emailSender.SendEmailAsync(email, subject, htmlMessage, null, null);
        }
    }
}
