import { Component, numberAttribute, OnInit } from '@angular/core';
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
  cart: cart[]=[];
  id: any=45;
  public displayedColumns: string[] =["name","description","price","imageData","Action"];


  constructor(public router: Router, public service: ServiceService) { }

  ngOnInit(): void {
    this.service.getAddToCart(this.id).subscribe(
      (res: any) => {
        console.log("Hello",res);
        this.router.navigate(['/cart']);
        const userId = localStorage.getItem('userId');
        if (!userId) {
          alert("No items in cart for this user");
        } else {
          this.service.cart = res as cart[];
          this.loadCartItems();
        }
      },
      () => alert("Failed to get item details")
    );
    this.loadCartItems();
  }
  
  loadCartItems(): void {
    const userId = localStorage.getItem('userId'); // Fetch userId from localStorage
    if (!userId){
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
  
  // getCardDetails(id: number): void {
  //   const userId = localStorage.getItem('userId'); 
  //   if (!userId) {
  //     alert("No user logged in");
  //     return;
  //   }
  //   this.service.getAddToCart(id).subscribe(
  //     (res: any) => {
  //       console.log("Hello",res);
  //       const userId = localStorage.getItem('userId');
  //       if (res.id != userId) {
  //         alert("No items in cart for this user");
  //       } else {
  //         this.loadCartItems();
  //       }
  //     },
  //     () => alert("Failed to get item details")
  //   );
  // }

  getCardDetails(id: number): void {
    const userId = localStorage.getItem('userId'); 
    if (!userId) {
      alert("No user logged in");
      return;
    }
    this.service.getAddToCart(id).subscribe(
      (res: any) => {
        console.log("Hello",res);
        this.router.navigate(['/cart']);
        const userId = localStorage.getItem('userId');
        if (!userId) {
          alert("No items in cart for this user");
        } else {
          this.service.cart = res as cart[];
          //this.loadCartItems();
        }
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

  removeItem(index: number) {
    // this.cartItems = this.cartItems.filter(item => item.imageId !== itemId);
    // this.calculateTotal();
    this.cartItems.splice(index, 1);
    this.calculateTotal();
  }

  home() {
    this.router.navigate(['/dashboard']);
  }

  onLogOut() {
    localStorage.removeItem("login");
    localStorage.removeItem("roleId");
    localStorage.removeItem("userId");
    this.router.navigate(["/login"]);
    //window.location.reload();
  }

}
