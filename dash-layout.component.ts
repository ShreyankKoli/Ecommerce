import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ServiceService } from '../../../service/service.service';
import { cart } from '../../../service/model.model';

@Component({
  selector: 'app-dash-layout',
  standalone: false,
  
  templateUrl: './dash-layout.component.html',
  styleUrl: './dash-layout.component.css'
})
export class DashLayoutComponent {
  isCarteVisible: boolean = false;
  loggedObj: any={};
  cartItems: any[] =[];
  id: any =45;
  cart: cart[]=[];

  constructor(public router: Router, public service: ServiceService){
    const localData = localStorage.getItem("login");
    if(localData != null){
      // const parseObj = JSON.parse(localData);
      // this.loggedObj = parseObj;
      // this.getCartData(this.loggedObj.userId);
    }   
  }

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
  

  showCart(){
    this.isCarteVisible = !this.isCarteVisible
    this.router.navigate(['/cart']);
  }

  onLogOut(){
    localStorage.removeItem("login");
    localStorage.removeItem("roleId");
    this.router.navigate(["/login"]);
  }

  // getCartData(id:number){
  //   this.service.getAddToCart(id)
  //   .subscribe((res:any)=>{
  //     this.cartItems = res.model;
  //   })
  // }
}
