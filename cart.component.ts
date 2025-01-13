import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../../../service/service.service';
import { Model } from '../../../service/model.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-cart',
  standalone: false,
  
  templateUrl: './cart.component.html',
  styleUrl: './cart.component.css'
})
export class CartComponent implements OnInit {
  data:Model[] = [];
  public dataSource: any = [];
  public displayedColumns: string[] = ["imageName", "imageDescription", "price", "imageData","Action"];
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

  ngOnInit(): void {
    this.data = this.service.getData();
    console.log(this.data);
  }

  getCartData(id:number){
    this.service.getAddToCart(id)
    .subscribe((res:any)=>{
      this.cartItems = res.model;
    })
  }

  onDelete(){}

}
