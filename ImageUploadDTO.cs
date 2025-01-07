namespace Ecommmerce.DTO
{
    public class ImageUploadDTO
    {
        public int RoleId { get; set; }
        public string ImageName { get; set; }
        public IFormFile File { get; set; }
    }
}
