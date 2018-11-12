using Microsoft.AspNetCore.Http;

namespace KombitServer.Models.Upload
{
    public class UploadModel
    {
        public int UserId { get; set; }
        public string Type { get; set; }
        public string UseCase { get; set; }
        public string ProductName { get; set; }
        public IFormFile File { get; set; }
    }
}
