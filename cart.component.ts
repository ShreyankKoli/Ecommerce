addCart(item: Model): void { 
  const localData = localStorage.getItem('login');
  const userData = localData ? JSON.parse(localData) : null;
  const userId = userData?.id || 0;

  if (userId === 0) {
    alert('User not logged in. Please log in to add items to the cart.');
    this.router.navigate(['/login']);
    return;
  }

  // Get specific data for the clicked item (if needed for further validation)
  const localStorageItems = localStorage.getItem('items'); // Assuming localStorage contains 'items' array
  const parsedItems = localStorageItems ? JSON.parse(localStorageItems) : [];
  const clickedItem = parsedItems.find((data: any) => data.id === item.id);

  if (!clickedItem) {
    alert('Item not found in local storage.');
    return;
  }

  // Prepare the cart object
  const cartObj = {
    ...clickedItem, // Include all details from the clicked item
    userId: userId,
    quantity: 1 // Default quantity
  };

  // Add the cartObj to the user's cart (localStorage or API call)
  const cartData = localStorage.getItem('cart');
  const cart = cartData ? JSON.parse(cartData) : [];
  cart.push(cartObj);

  // Update the cart in localStorage
  localStorage.setItem('cart', JSON.stringify(cart));
  alert('Item added to cart successfully!');
}
