using GalleryApp.IServices;
using GalleryApp.Services.Service;
using System.Net.Mail;

namespace GalleryApp.Services
{
    public class MailServices : IMailServices
    {
        public async Task<string> SendMail(Mail email)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(email.FromMailId);
                    email.ToMailIds.ForEach(x =>
                    {
                        mail.To.Add(x);
                    });
                    mail.Subject = email.Subject;
                    mail.Body = email.Body;
                    mail.IsBodyHtml = true;
                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.Credentials = new System.Net.NetworkCredential(email.FromMailId, email.ToMailIdPassword);
                        smtp.EnableSsl = true;
                        await smtp.SendMailAsync(mail);
                        return "Mail sent.";
                    }
                }
            }
            catch (Exception ex)
            {

                return ex.Message;
            }
        }
    }
}
