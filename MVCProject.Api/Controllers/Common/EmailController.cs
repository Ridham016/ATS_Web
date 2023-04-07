using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using MimeKit.Text;
using MVCProject.Api.Models;
using MVCProject.Api.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace MVCProject.Api.Controllers
{
    public class EmailController : BaseController
    {
        [HttpPost]
        public ApiResponse SendEmail(string applicantemail, string body)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("lelia.goodwin70@ethereal.email"));
            email.To.Add(MailboxAddress.Parse(applicantemail));
            email.Subject = "Test Email";
            email.Body = new TextPart(TextFormat.Html) { Text = body};

            using var smtp = new SmtpClient();
            smtp.Connect("smtp.ethereal.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("lelia.goodwin70@ethereal.email", "JKNDCFBnK7RDe6HZDz");
            smtp.Send(email);
            smtp.Disconnect(true);

            return this.Response(MessageTypes.Success, message: "Email Sent Successfully");
        }
    }
}
