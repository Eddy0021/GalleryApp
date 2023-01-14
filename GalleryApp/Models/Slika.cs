namespace GalleryApp.Models
{
    public class Slika
    {
        public int PhotoID { get; set; }
        public int AlbumID { get; set; }
        public string Photo_Name { get; set; }
        public IFormFile Files { get; set; } = null;
        public string imageURL { get; set; }

    }
}
