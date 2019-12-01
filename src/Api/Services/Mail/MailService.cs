using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Api.Configuration;

namespace Api.Services.Mail
{
    class MailService : IMailService
    {
        private readonly MailConfiguration _mailConfiguration;

        public MailService(MailConfiguration mailConfiguration)
        {
            _mailConfiguration = mailConfiguration;
        }

        public async  Task<bool> SendMail(string from, string subject, string content)
        {
            try
            {
                var mailClient = new SmtpClient(_mailConfiguration.Domain, _mailConfiguration.Port)
                {
                    Credentials = new NetworkCredential(_mailConfiguration.UserName, _mailConfiguration.Password),
                    EnableSsl = true
                };
                await mailClient.SendMailAsync(from, _mailConfiguration.Address, subject, content);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}