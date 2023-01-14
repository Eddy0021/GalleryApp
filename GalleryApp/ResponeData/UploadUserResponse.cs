namespace GalleryApp.ResponeData
{
    public class UploadUserResponse
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set;} = DateTime.Now;
        public string Message { get; set; } = string.Empty;
    }
}
