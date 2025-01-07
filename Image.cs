namespace Ecommmerce.Models
{
    public partial class Image
    {
        public int ImageId { get; set; }

        public int RoleId { get; set; } // Change to non-nullable type if RoleId is required

        public string ImageName { get; set; } = null!;

        public byte[] ImageData { get; set; } = null!;

        public virtual UserRole Role { get; set; }
    }
}
