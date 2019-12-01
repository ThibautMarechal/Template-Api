using System.Net;
using System.Threading.Tasks;
using Api.Services.Mail;
using Contract;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("/contact")]
    public class ContactController : ControllerBase
    {
        private readonly IMailService _mailService;

        public ContactController(IMailService mailService)
        {
            _mailService = mailService;
        }

        [HttpPost("mail")]
        public async Task<IActionResult> SendMailAsync([FromBody]MailIn mail)
        {
            var mailSend = await _mailService.SendMail(mail.From, mail.Subject, mail.Content).ConfigureAwait(false);
            if (!mailSend) 
                return StatusCode((int)HttpStatusCode.InternalServerError);
            return Accepted();
        }
    }
}