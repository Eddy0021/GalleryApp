namespace GalleryApp.ResponeData
{
    public class UploadAlbumResponse
    {
        public string Album_Name { get; set; }
        public int UserID { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
