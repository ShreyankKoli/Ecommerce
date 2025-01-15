[HttpPut("updateCartItemQuantity")]
public async Task<ActionResult<UpdateCartDTO>> EditCartQuantity([FromBody] UpdateCartDTO updateCartDTO)
{
    // Fetch the cart item from the database using the provided CartId
    var cartItem = await _context.Carts.FirstOrDefaultAsync(a => a.CartId == updateCartDTO.CartId);
    
    // Check if the cart item exists
    if (cartItem == null)
    {
        return NotFound(new { Message = "Cart item not found." });
    }

    // Update the quantity
    cartItem.Quantity = updateCartDTO.Quantity;

    // Save changes to the database
    _context.Carts.Update(cartItem);
    await _context.SaveChangesAsync();

    // Return the updated DTO as the response
    return Ok(updateCartDTO);
}

public class UpdateCartDTO
{
    public int CartId { get; set; }
    public int Quantity { get; set; }
}

editCart(cartId: number, quantity: number): Observable<any> {
  const url = `https://localhost:7016/api/User/updateCartItemQuantity`;
  return this.http.put(url, { cartId, quantity });
}

updateQuantity(item: Model, quantity: number): void {
  if (quantity < 1) return; // Ensure the quantity is valid before making the API call

  this.service.editCart(item.cartId, quantity).subscribe({
    next: (res: any) => {
      item.quantity = quantity; // Update the UI with the new quantity
      this.calculateTotal(); // Recalculate the total
      console.log('Quantity updated successfully:', res);
    },
    error: (err) => {
      console.error('Error updating quantity:', err);
    }
  });
}




