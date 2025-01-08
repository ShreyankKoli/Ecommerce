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

[HttpGet("get/all")]
public IActionResult GetAllImages()
{
    var images = _context.Images.ToList();

    if (!images.Any())
    {
        return NotFound("No images found.");
    }

    // Return a list of image data (e.g., IDs, names, or other metadata)
    var imageList = images.Select(i => new
    {
        i.ImageId,
        i.ImageName, // Assuming ImageName exists in your model
        ImageUrl = Url.Action(nameof(GetImage), new { id = i.ImageId }) // Generate URLs for each image
    }).ToList();

    return Ok(imageList);
}


