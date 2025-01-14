import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../../../service/service.service';
import { cart, Model } from '../../../service/model.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart',
  standalone: false,
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cartItems: Model[] = [];
  totalPrice: number = 0;
  public displayedColumns: string[] = ["name", "description", "price", "imageData", "quantity", "Action"];

  constructor(public router: Router, public service: ServiceService) { }

  ngOnInit(): void {
    const userId = localStorage.getItem('userId'); // Fetch userId from localStorage
    if (!userId) {
      alert("No user logged in");
      return;
    }

    this.service.getAddToCart(userId).subscribe(
      (res: Model[]) => {
        this.cartItems = res; // Store fetched cart items
        this.calculateTotal(); // Calculate total price of cart items
      },
      () => alert("Failed to get cart items for this user")
    );
  }

  calculateTotal(): void {
    this.totalPrice = this.cartItems.reduce((acc, item) => acc + (item.price * item.quantity), 0);
  }

  updateQuantity(img: Model, quantity: number): void {
    if (quantity < 1) return;

    img.quantity = quantity;
    this.calculateTotal();

    // Optionally, update the quantity in the backend
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.service.updateCartItemQuantity(userId, img.cartId, quantity).subscribe(
        () => console.log(`Updated quantity for cart item ${img.cartId}`),
        (err) => console.error("Error updating quantity:", err)
      );
    }
  }

  removeItem(index: number): void {
    const itemId = this.cartItems[index].cartId;
    this.cartItems.splice(index, 1);
    this.calculateTotal();

    // Optionally, remove the item from the backend
    const userId = localStorage.getItem('userId');
    if (userId) {
      this.service.removeItemFromCart(userId, itemId).subscribe(
        () => console.log(`Item ${itemId} removed from cart`),
        (err) => console.error("Error removing item:", err)
      );
    }
  }

  home(): void {
    this.router.navigate(['/dashboard']);
  }

  onLogOut(): void {
    localStorage.removeItem("login");
    localStorage.removeItem("roleId");
    localStorage.removeItem("userId");
    this.router.navigate(["/login"]);
  }
}
