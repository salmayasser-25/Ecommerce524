using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Build.Tasks;
using System.Net;
using System.Net.Mail;

namespace Ecommerce524.Utilities
{
    public class EmailSender : IEmailSender
    {
        private readonly string _clientId = "your.client.id";
        private readonly string _clientSecret = "your.client.secret";

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("salmays287@gmail.com\r\n", "jwbf mofr khcp suzp")
            };
            return client.SendMailAsync(
                new MailMessage(from: "salmays287@gmail.com",
                to: email,
                subject,
                htmlMessage)
                {
                    IsBodyHtml = true

                });
        }
    }
}