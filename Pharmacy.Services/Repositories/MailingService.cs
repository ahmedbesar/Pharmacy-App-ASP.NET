using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using MimeKit;
using Pharmacy.Domian.Interfaces;
using Pharmacy.Services.Settings;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace Pharmacy.Services.Repositories
{
    public class MailingService : IMailingService
    {
        private readonly MailSettings _mailSettings;

        public MailingService(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string mailTo, string subject, string body, IList<IFormFile> attachments = null)
        {

            try
            {
                var email = new MimeMessage
                {
                    Sender = MailboxAddress.Parse(_mailSettings.Email),
                    Subject = subject
                };

                email.To.Add(MailboxAddress.Parse(mailTo));

                var builder = new BodyBuilder();

                if (attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in attachments)
                    {
                        if (file.Length > 0)
                        {
                            using var ms = new MemoryStream();
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();

                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }
                const int timeout = 5000000; // 5 seconds

                // Create a CancellationTokenSource with the specified timeout
                using var cts = new CancellationTokenSource(timeout);
                cts.CancelAfter(timeout);

                builder.HtmlBody = body;
                email.Body = builder.ToMessageBody();
                email.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Email));

                using var smtp = new SmtpClient();
                smtp.Connect("smtp.gmail.com", 465, SecureSocketOptions.SslOnConnect,cts.Token);
                smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);
                await smtp.SendAsync(email);

                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }




            
        }
    }
}