import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ServiceService } from '../../../service/service.service';

@Component({
  selector: 'app-dash-layout',
  standalone: false,
  
  templateUrl: './dash-layout.component.html',
  styleUrl: './dash-layout.component.css'
})
export class DashLayoutComponent {
  isCarteVisible: boolean = false;
  loggedObj: any={};
  cartItems: any[] =[]

  constructor(public router: Router, public service: ServiceService){
    const localData = localStorage.getItem("login");
    if(localData != null){
      // const parseObj = JSON.parse(localData);
      // this.loggedObj = parseObj;
      this.getCartData(this.loggedObj.userId);
    }
   
  }

  showCart(){
    this.isCarteVisible = !this.isCarteVisible
  }

  onLogOut(){
    localStorage.removeItem("login");
    localStorage.removeItem("roleId");
    this.router.navigate(["/login"]);
  }

  getCartData(id:number){
    this.service.getAddToCart(id)
    .subscribe((res:any)=>{
      this.cartItems = res.model;
    })
  }
}
