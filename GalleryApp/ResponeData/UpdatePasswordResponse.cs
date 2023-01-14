namespace GalleryApp.ResponeData
{
    public class UpdatePasswordResponse
    {
        public string GUID { get; set; }
        public string Password { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public int Counter { get; set; }
    }
}
