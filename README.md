import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../../../service/service.service';
import { Model } from '../../../service/model.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'] // Fix typo: 'styleUrl' -> 'styleUrls'
})
export class DashboardComponent implements OnInit {
  model: Model[] = []; // Holds fetched product data
  cartCount: number = 0; // Tracks the number of items in the cart

  constructor(private service: ServiceService, private router: Router) {}

  ngOnInit(): void {
    this.fetchImages(); // Fetch product data on component initialization
  }

  fetchImages(): void {
    this.service.getImage().subscribe({
      next: (res) => {
        this.model = res as Model[];
        console.log(res);
      },
      error: (err) => {
        console.error('Error fetching images:', err);
      }
    });
  }

  addCart(item: Model): void {
    const localData = localStorage.getItem('login');
    const userData = localData ? JSON.parse(localData) : null;
    const userId = userData?.id || 0;

    if (userId === 0) {
      alert('User not logged in. Please log in to add items to the cart.');
      this.router.navigate(['/login']);
      return;
    }

    // Prepare the cart object
    const cartObj = {
      ...item, // Include all details (imageId, imageName, price, etc.)
      userId: userId,
      quantity: 1 // Default quantity
    };

    // Send the cart object to CartComponent via service
    this.service.addToCart(cartObj).subscribe({
      next: (res: any) => {
        alert('Item added to cart successfully!');
        this.service.sharedData(cartObj); // Share cart data
        this.router.navigate(['/cart']); // Navigate to CartComponent
      },
      error: (err: any) => {
        console.error('Error adding item to cart:', err);
        alert('Failed to add item to cart. Please try again later.');
      }
    });
  }
}

<div class="dashboard-container">
  <div *ngFor="let item of model" class="product-card">
    <img [src]="item.imageData" alt="{{ item.imageName }}" />
    <h3>{{ item.imageName }}</h3>
    <p>{{ item.imageDescription }}</p>
    <p>Price: {{ item.price | currency }}</p>
    <button (click)="addCart(item)">Add to Cart</button>
  </div>
</div>

import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../../../service/service.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {
  cartItems: any[] = []; // Holds all cart items
  totalPrice: number = 0; // Holds the total price

  constructor(private service: ServiceService) {}

  ngOnInit(): void {
    this.loadCartItems();
  }

  loadCartItems(): void {
    // Get cart data from the shared service
    this.service.getCartData().subscribe((items: any[]) => {
      this.cartItems = items;
      this.calculateTotal();
    });
  }

  calculateTotal(): void {
    this.totalPrice = this.cartItems.reduce((acc, item) => acc + item.price * item.quantity, 0);
  }

  updateQuantity(item: any, quantity: number): void {
    if (quantity < 1) return; // Prevent negative quantities

    item.quantity = quantity;
    this.calculateTotal();
  }

  removeItem(itemId: number): void {
    this.cartItems = this.cartItems.filter(item => item.imageId !== itemId);
    this.calculateTotal();
  }
}

<div class="cart-container">
  <h2>Your Cart</h2>
  <div *ngFor="let item of cartItems" class="cart-item">
    <h3>{{ item.imageName }}</h3>
    <p>{{ item.imageDescription }}</p>
    <p>Price: {{ item.price | currency }}</p>
    <input
      type="number"
      [value]="item.quantity"
      (change)="updateQuantity(item, $event.target.value)"
    />
    <button (click)="removeItem(item.imageId)">Remove</button>
  </div>
  <h3>Total Price: {{ totalPrice | currency }}</h3>
</div>
.cart-container {
  padding: 20px;
}

.cart-item {
  border: 1px solid #ccc;
  padding: 10px;
  margin-bottom: 15px;
  display: flex;
  justify-content: space-between;
  align-items: center;
}


import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ServiceService {
  private cartData = new BehaviorSubject<any[]>([]); // Holds the cart data

  constructor() {}

  sharedData(data: any): void {
    const currentCart = this.cartData.value;
    currentCart.push(data);
    this.cartData.next(currentCart);
  }

  getCartData(): Observable<any[]> {
    return this.cartData.asObservable();
  }
}





