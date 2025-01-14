import { Component, OnInit, SecurityContext } from '@angular/core';
import { ServiceService } from '../../../service/service.service';
import { Model } from '../../../service/model.model';
import { NgFor, NgStyle } from '@angular/common';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
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
  cartCount: number = 0;
  loggedObj: any = {};
  cartItems: Model[] = []
  item: number = 0;

  constructor(private service: ServiceService, public router: Router) {
    // const localData = localStorage.getItem('login');
    // if(localData != null){
    //   this.getCartData(this.loggedObj.roleId);

    //   //alert('localstorage is not null')
    // }
  }

  ngOnInit(): void {
    this.fetchImages();
  }

  fetchImages(): void {
    this.service.getImage().subscribe({
      next: (res) => {
        this.model = res as Model[];
        console.log(res);
        // const imageId = this.model.map(item => item.imageId);
        // localStorage.setItem('imadeId', JSON.stringify(imageId));

        // localStorage.setItem('image', JSON.stringify(res));

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

  // plusCart(imageId:any){
  //   console.log(imageId);
  //   this.cartCount += 1;
  // }

  // SendData(imageId:any):void{
  //   this.service.sharedData(this.model);
  //   this.router.navigate(['/cart']);
  // }

  addCart(item: Model): void {
    const localData = localStorage.getItem('login');
    // const dataLocal = localStorage.getItem('imadeId');
    // const localImage = localStorage.getItem('image');
    // const imageLocal = localImage ? JSON.parse(localImage) : null;
    //const imageData = dataLocal ? JSON.parse(dataLocal) : null;
    const userData = localData ? JSON.parse(localData) : null;
    const userId = userData?.userId || 0;
    //const imageId = imageData?.imageId;
    // const imageName = imageLocal?.imageName;
    //const imageDescription = userData?.imageDescription;
    //const price = userData?.price;
    //const imageData = userData?.imageData;
    const roleId = item.roleId;
    //const userId = item.userId
    const image = item.imageId;
    const imageName = item.imageName
    const imageData = item.imageData
    const price = item.price
    const imageDescription = item.imageDescription

    if (userId === 0) {
      alert('User not logged in');
      this.router.navigate(['/login']);
      return;
    }
    const cartObj = {
      imageId: image,
      imageName: imageName,
      imageDescription:imageDescription,
      price:price,
      imageData:imageData,
      userId: userId,
      roleId: roleId,
      quantity: 1
    };
    this.service.addToCart(cartObj).subscribe({
      next: (res: Model[]) => {
        alert('Item added to cart successfully!');
        this.service.sharedData(cartObj);
        //localStorage.setItem('cartObj',JSON.stringify(cartObj));
        console.warn("Hello");
        let user = localStorage.getItem('userId');
        let userId = user && JSON.stringify(user);
        console.warn(userId)
        console.log(cartObj);
        this.router.navigate(['/cart']);
      },
      error: (err: any) => {
        console.error('Error adding item to cart:', err);
        alert('Failed to add item to cart');
      }
    });
  }

  // removeCart(imageId:any){
  //   if(this.cartCount >= 1){
  //     this.cartCount -= 1;
  //     console.log(imageId);
  //   }
  // }

  // getCartData(id:number){
  //   this.service.getAddToCart(id)
  //   .subscribe((res:any)=>{
  //     this.cartItems = res as Model[];
  //   })
  // }



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
