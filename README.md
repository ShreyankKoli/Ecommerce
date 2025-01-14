public class CartDto
{
    public int ImageId { get; set; }
    public int CartId { get; set; }
    public string ImageName { get; set; }
    public string ImageDescription { get; set; }
    public decimal Price { get; set; }
    public int UserId { get; set; }
}


[HttpGet("{id}")]
public async Task<ActionResult<List<CartDto>>> GetCart(int id)
{
    try
    {
        // Fetch cart items for the given user ID
        var data = await (from cart in context.Carts
                          join image in context.Images
                          on cart.ImageId equals image.ImageId
                          where cart.UserId == id
                          select new CartDto
                          {
                              ImageId = image.ImageId,
                              CartId = cart.CartId,
                              ImageName = image.ImageName,
                              ImageDescription = image.ImageDescription,
                              Price = image.Price,
                              UserId = cart.UserId
                          }).ToListAsync();

        // Check if data exists
        if (data == null || data.Count == 0)
        {
            return NotFound($"No cart items found for user ID {id}");
        }

        return Ok(data);
    }
    catch (Exception ex)
    {
        // Log the exception and return a server error response
        return StatusCode(500, $"Internal server error: {ex.Message}");
    }
}
