using Ecommmerce.DTO;
using Ecommmerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace Ecommmerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly EcommerceContext _context;

        public ImageController(EcommerceContext context)
        {
            _context = context;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] ImageUploadDTO imageUploadDto)
        {
            if (imageUploadDto.File == null || imageUploadDto.File.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            // Convert the uploaded file to a byte array
            using (var memoryStream = new MemoryStream())
            {
                await imageUploadDto.File.CopyToAsync(memoryStream);
                var imageBytes = memoryStream.ToArray();

                // Create a new Image entity
                var image = new Image
                {
                    RoleId = imageUploadDto.RoleId,
                    ImageName = imageUploadDto.ImageName,
                    ImageData = imageBytes
                };

                // Add the image to the database
                _context.Images.Add(image);
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Image uploaded successfully", ImageId = image.ImageId });
            }
        }

        [HttpGet("get/{id}")]
        public IActionResult GetImage(int id)
        {
            var image = _context.Images.FirstOrDefault(i => i.ImageId == id);

            if (image == null)
            {
                return NotFound("Image not found.");
            }

            // Return image as byte array
            return File(image.ImageData, "image/jpeg");  // Assuming the image is JPEG, change mime type if necessary
        }
    }
}

