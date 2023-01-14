using GalleryApp.DTO;
using GalleryApp.Models;
using GalleryApp.ResponeData;

namespace GalleryApp.IRepository
{
    public interface IUserRepository
    {
        Task<User> Create(UploadUserResponse user);

        Task<User> GetByUsernamePassword(LoginDTO request);
        Task<string> ChangePasswordRequest(CheckEmailResponse user);

        Task<string> Update(UpdatePasswordResponse user);
    }
}
