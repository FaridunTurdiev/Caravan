using CaravanApi.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace CaravanApi.Services
{
    public class ResetPasswordService : IEmailService
    {
        private readonly IConfiguration _config;
        public ResetPasswordService(IConfiguration configuration)
        {
            _config = configuration;
        }
        public void SendEmail(EmailModel emailModel)
        {
            var emailMessage = new MimeMessage();
            var from = _config["EmailSettings:From"];
            emailMessage.From.Add(new MailboxAddress("Caravan", from));
            emailMessage.To.Add(new MailboxAddress(emailModel.MailReciever, emailModel.MailReciever));
            emailMessage.Subject = emailModel.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(emailModel.Content)
            };
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_config["EmailSettings:SmtpServer"], 465, true);
                    client.Authenticate(_config["EmailSettings:From"], _config["EmailSettings:Password"]);
                    client.Send(emailMessage);
                }
                catch (Exception ex)
                {

                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
