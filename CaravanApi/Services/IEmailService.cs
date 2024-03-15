using CaravanApi.Models;

namespace CaravanApi.Services
{
    public interface IEmailService
    {
        void SendEmail(EmailModel emailModel);
    }
}