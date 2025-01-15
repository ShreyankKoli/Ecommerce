editCart(cartId: any, quantity: any):Observable<any>{
  // const formData = new FormData();
  // formData.append('cartId',cartId.toString());
  // formData.append('quantity',quantity.toString());
  return this.http.put('https://localhost:7016/api/User/updateCartItemQuantity'+cartId+quantity,{cartId: cartId, quantity: quantity});
}

updateQuantity(img:Model,quantity:number):void{
  this.service.editCart(img.cartId,img.quantity).subscribe((res:Model[])=>{
    //let user = localStorage.getItem('cartId');
    if(img.cartId !== null && img.cartId == img.cartId)
    {
    if (quantity < 1) return;
    img.quantity = quantity;
    this.calculateTotal();
    console.log("edited",res);
    }
  })
}

[HttpPut("updateCartItemQuantity")]
public async Task<ActionResult<UpdateCartDTO>> EditCartQuantity([FromBody] UpdateCartDTO updateCartDTO)
{
    var cartItem = await context.Carts.Where(a => a.CartId == updateCartDTO.CartId).FirstOrDefaultAsync();
    if(cartItem == null)
    {
        return NotFound();
    }
    cartItem.Quantity =updateCartDTO.Quantity;
    context.Add(cartItem);
    await context.SaveChangesAsync();
    return Ok();
}
