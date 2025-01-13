import { Component,OnInit, SecurityContext } from '@angular/core';
import { ServiceService } from '../../../service/service.service';
import { Model } from '../../../service/model.model';
import { NgFor,NgStyle } from '@angular/common';
import { DomSanitizer,SafeResourceUrl } from '@angular/platform-browser';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  //base64Images: { [key: number]: string } = {};
  model: Model[] = [];
  cartCount:number=0;
  loggedObj: any=[];

  constructor(private service: ServiceService, public router: Router) {
    const localData = localStorage.getItem('login');
    if(localData != null){
      
      //alert('localstorage is not null')
    }
  }

  ngOnInit(): void {
    this.fetchImages();
  }

  fetchImages(): void {
    this.service.getImage().subscribe({
      next: (res) => {
        this.model = res as Model[];
        console.log(res);
        // this.model.forEach((img) => {
        //   if (img.imageData) {
        //     this.convertImageToBase64(img.imageData, img.imageId);
        //   }
        // });
      },
      error: (err) => {
        console.error('Error fetching images:', err);
      },
    });
  }

  // addCart(imageId:any){
  //   console.log(imageId);
  //   this.cartCount += 1;
  // }

  // SendData(imageId:any):void{
  //   this.service.sharedData(this.model);
  //   this.router.navigate(['/cart']);
  // }

  addCart(imageId:number){
    const obj ={
      "cartId": 0,
      "imageId": imageId,
      "roleId": 102,
      "userId": 45,
      "quantity": 1
    }
    this.service.addToCart(obj).subscribe((res:any)=>{
      alert("data added");
        this.service.sharedData(obj);
        this.router.navigate(['/cart']);
      // this.loggedObj = res.model;
      // localStorage.setItem('app_user',JSON.stringify(res.model));
    })
  }

  removeCart(imageId:any){
    if(this.cartCount >= 1){
      this.cartCount -= 1;
      console.log(imageId);
    }
  }



  // convertImageToBase64(imageData: any, imageId: number): void {
  //   const reader = new FileReader();
  //   reader.onload = () => {
  //     this.base64Images[imageId] = reader.result as string;
  //   };
  //   reader.onerror = (error) => {
  //     console.error('Error converting image to base64:', error);
  //   };
    
  //   const blob = imageData instanceof Blob ? imageData : new Blob([imageData]);
  //   reader.readAsDataURL(blob);
  // }

}
