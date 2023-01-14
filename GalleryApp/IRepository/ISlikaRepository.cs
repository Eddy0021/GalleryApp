using GalleryApp.Models;
using GalleryApp.ResponeData;

namespace GalleryApp.IRepository
{
    public interface ISlikaRepository
    {
        Task<Slika> Upload(UploadSlikaResponse slika);

        Task<Slika> Update(UpdateSlikaResponse slika);

        Task<List<Slika>> GetAllByAlbumID(int id);

        Task<string> Delete(int id);
    }
}
