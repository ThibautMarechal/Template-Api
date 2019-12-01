using System.Threading.Tasks;

namespace Api.Services.Mail
{
    public interface IMailService
    {
        Task<bool> SendMail(string from, string subject, string content);
    }
}