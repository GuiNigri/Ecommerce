using EcommercePrestige.Model.Entity;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading.Tasks;
using EcommercePrestige.Model.Interfaces.Services;
using MailKit.Net.Smtp;
using MimeKit;

namespace EcommercePrestige.EmailApi
{
    public class AuthMessageSender:IEmailSenderServices
    {
        private readonly EmailSettingsModel _emailSettings;
        public AuthMessageSender(IOptions<EmailSettingsModel> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        

        public Task SendEmailAsync(string email, string subject, string message, byte[] anexo, string nomeArquivo)
        {
            try
            {
                Execute(email, subject, message, anexo, nomeArquivo).Wait();
                return Task.FromResult(0);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task Execute(string email, string subject, string message, byte[] anexo, string nomeArquivo)
        {
            try
            {
                var toEmail = email;

                var mail = new MimeMessage();

                mail.From.Add(new MailboxAddress("Prestige do Brasil", _emailSettings.UsernameEmail));
                mail.To.Add(new MailboxAddress(toEmail, toEmail));

                mail.Subject = subject;
                mail.Priority = MessagePriority.Normal;

                //outras opções

                if (anexo != null)
                {
                    var builder = new BodyBuilder { HtmlBody = message };

                    builder.Attachments.Add($"{nomeArquivo}.pdf", anexo);

                    mail.Body = builder.ToMessageBody();
                }
                else
                {
                    mail.Body = new TextPart("html") { Text = message };
                }


                using var client = new SmtpClient();

                await client.ConnectAsync(_emailSettings.PrimaryDomain, _emailSettings.PrimaryPort, true);

                await client.AuthenticateAsync(_emailSettings.UsernameEmail, _emailSettings.UsernamePassword);

                await client.SendAsync(mail);

                await client.DisconnectAsync(true);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
