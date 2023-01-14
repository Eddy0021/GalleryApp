namespace GalleryApp.ResponeData
{
    public class UploadSlikaResponse
    {
        public int AlbumID { get; set; }
        public string Photo_Name { get; set; }
        public IFormFile Files { get; set; } = null;
        public string imageURL { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
