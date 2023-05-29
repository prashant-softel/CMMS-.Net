using CMMSAPIs.Models.Mails;
using CMMSAPIs.Repositories;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
namespace CMMSAPIs.BS.Mails
{
    public interface IMailService
    {
        Task<List<CMMailResponse>> SendEmailAsync(CMMailRequest mailRequest, CMMailSettings _mailSettings);
    }
    //public class MailService : IMailService
    public class MailService 
    {
        //private readonly CMMailSettings _mailSettings;
        //public MailService(IOptions<CMMailSettings> mailSettings)
        //{
        //    _mailSettings = mailSettings.Value;
        //}

        public static async Task<List<CMMailResponse>> SendEmailAsync(CMMailRequest mailRequest, CMMailSettings _mailSettings)
        {
            try
            {
                List<CMMailResponse> _MailResponse = new List<CMMailResponse>();

                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                foreach (var mail in mailRequest.ToEmail)
                {
                    email.To.Add(MailboxAddress.Parse(mail));
                }
                foreach (var mail in mailRequest.CcEmail)
                {
                    email.To.Add(MailboxAddress.Parse(mail));
                }
                // email.Cc.Add(MailboxAddress.Parse(mailRequest.CcEmail));
                email.Subject = mailRequest.Subject;
                //email.Headers = mailRequest.Headers;
                var builder = new BodyBuilder();
                if (mailRequest.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in mailRequest.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }
                builder.HtmlBody = mailRequest.Body;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                _MailResponse.Add(new CMMailResponse { mail_sent = true, message = "Mail sent successfully" });
                return _MailResponse;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
