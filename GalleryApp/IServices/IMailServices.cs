using GalleryApp.Services.Service;

namespace GalleryApp.IServices
{
    public interface IMailServices
    {
        Task<string> SendMail(Mail mail);
    }
}
