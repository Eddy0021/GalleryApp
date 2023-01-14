using GalleryApp.Models;
using GalleryApp.ResponeData;

namespace GalleryApp.IRepository
{
    public interface IAlbumRepository
    {
        Task<Album> Create(UploadAlbumResponse album);

        Task<Album> Update(UpdateAlbumResponse album);

        Task<List<Album>> GetAlbumByUserId(int id);

        Task<string> Delete(int id);
    }
}
