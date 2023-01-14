namespace GalleryApp.ResponeData
{
    public class UpdateSlikaResponse
    {
        public int PhotoID { get; set; }
        public string Photo_Name { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
