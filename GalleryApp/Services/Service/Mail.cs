namespace GalleryApp.Services.Service
{
    public class Mail
    {
        public string FromMailId { get; set; } = /*"youremail@gmail.com"*/"";
        public string ToMailIdPassword { get; set; } = /*"YourPassword"*/"";
        public List<string> ToMailIds { get; set; } = new List<string>();
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }
}
