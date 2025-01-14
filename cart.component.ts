import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../../../service/service.service';
import { cart, Model } from '../../../service/model.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart',
  standalone: false,
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {
  cartItems: Model[] = [];
  totalPrice: number = 0;
  cart: cart[] = [];
  public displayedColumns: string[] = ["name", "description", "price", "imageData", "Action"];

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

  loadCartItems(): void {
    const userId = localStorage.getItem('userId'); // Fetch userId from localStorage
    if (!userId) {
      alert("No data in cart");
      return;
    }
    this.service.getCartData().subscribe(
      (items: Model[]) => {
        this.cartItems = items;
        this.calculateTotal();
      },
      () => alert("Failed to load cart items")
    );
  }

  getCardDetails(id: number): void {
    const userId = localStorage.getItem('userId'); 
    if (!userId) {
      alert("No user logged in");
      return;
    }

    this.service.getAddToCart(userId).subscribe(
      (res: Model[]) => {
        this.cartItems = res; // Update cart items
        this.calculateTotal(); // Recalculate total price
      },
      () => alert("Failed to get item details")
    );
  }

  calculateTotal(): void {
    this.totalPrice = this.cartItems.reduce((acc, item) => acc + item.price * item.quantity, 0);
  }

  updateQuantity(img: any, quantity: number): void {
    if (quantity < 1) return;

    img.quantity = quantity;
    this.calculateTotal();
  }

  removeItem(index: number): void {
    this.cartItems.splice(index, 1);
    this.calculateTotal();
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
