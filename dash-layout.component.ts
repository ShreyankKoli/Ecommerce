import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ServiceService } from '../../../service/service.service';
import { cart } from '../../../service/model.model';

@Component({
  selector: 'app-dash-layout',
  standalone: false,
  templateUrl: './dash-layout.component.html',
  styleUrls: ['./dash-layout.component.css']
})
export class DashLayoutComponent {
  isCartVisible: boolean = false;  // Renamed for clarity
  loggedObj: any = {};
  cartItems: any[] = [];
  cart: cart[] = [];

  constructor(public router: Router, public service: ServiceService) {
    const localData = localStorage.getItem("login");
    if (localData != null) {
      // This block is unused, you might want to use it for further authentication checks
      // const parseObj = JSON.parse(localData);
      // this.loggedObj = parseObj;
    }
  }

  // Fetch cart details for a logged-in user
  getCardDetails(): void {
    const userId = localStorage.getItem('userId');
    
    if (!userId) {
      alert("No user logged in");
      this.router.navigate(['/login']);
      return;
    }

    this.service.getAddToCart(userId).subscribe(
      (res: any) => {
        console.log("Cart items fetched:", res);
        this.cartItems = res;  // Store cart items received from the service
        this.router.navigate(['/cart']);
      },
      (err) => {
        console.error("Error fetching cart details", err);
        alert("Failed to get item details");
      }
    );
  }

  // Toggle visibility of the cart
  showCart(): void {
    this.isCartVisible = !this.isCartVisible;
    this.router.navigate(['/cart']);
  }

  // Log out the user and clear session data
  onLogOut(): void {
    localStorage.removeItem("login");
    localStorage.removeItem("roleId");
    localStorage.removeItem("userId");
    this.router.navigate(["/login"]);
  }
}
