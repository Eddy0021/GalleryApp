namespace GalleryApp.ResponeData
{
    public class UpdateAlbumResponse
    {
        public int AlbumID { get; set; }
        public string Album_Name { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
