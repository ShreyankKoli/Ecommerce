import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Mode } from 'fs';
import { cart, Model } from './model.model';
import { HttpClient, HttpHeaders ,HttpErrorResponse } from '@angular/common/http';
import { BehaviorSubject, catchError, Observable, Subject } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class ServiceService {
  url: string = environment.apiBaseURL;
  list: Model[]=[];
  cart: cart[]=[]
  //formData: Model = new Model();
  formsubmitted: boolean = false;
  handleError: any;
  private cartData = new BehaviorSubject<Model[]>([]);

  sharedData(data: any): void {
    const currentCart = this.cartData.value;
    currentCart.push(data);
    this.cartData.next(currentCart);
  }

  getCartData(){
    return this.cartData;
  }

  onAddToCart$: Subject<any> = new Subject();

  constructor(private http: HttpClient) { }

  getImage():Observable<Model[]>{
    return this.http.get<Model[]>('https://localhost:7016/api/User/get/all/image')
  }

  uploadImage(file: File, roleId: number, imageName: string,imageDescription: string,userId:number,price:string,quantity:number): Observable<any> {
    const formData = new FormData();
    formData.append('File', file); // 'File' matches the API's DTO property name
    formData.append('RoleId', roleId.toString());
    formData.append('ImageName', imageName);
    formData.append('ImageDescription',imageDescription);
    formData.append('Price',price);
    formData.append('UserId',userId.toString());
    formData.append('quantity',quantity.toString());

    return this.http.post('https://localhost:7016/api/User/upload', formData);
  }

  putList(roleId: number, imageName: string, imageDescription: string, userId: number, price: string, quantity: number): Observable<any> {
    const formData = new FormData();
    // formData.append('File', file); // 'File' matches the API's DTO property name
    formData.append('RoleId', roleId.toString());
    formData.append('ImageName', imageName);
    formData.append('ImageDescription',imageDescription);
    formData.append('Price',price);
    formData.append('UserId',userId.toString());
    return this.http.put('https://localhost:7016/api/User/Update',formData);
  }

  deleteList(id:number){
    return this.http.delete('https://localhost:7016/api/User/Delete'+'/'+id);
  }

  // addToCart(roleId: number,userId: number,quantity: number,imageId: number):Observable<any>{
  //   const formData = new FormData();
  //   formData.append('RoleId', roleId.toString());
  //   formData.append('UserId',userId.toString());
  //   formData.append('quantity',quantity.toString());
  //   formData.append('imageId',imageId.toString());
  //   return this.http.post('https://localhost:7016/api/User/addToCart',formData)
  // }

  addToCart(obj:any):Observable<any>{
    return this.http.post('https://localhost:7016/api/User/addToCart',obj)
  }

  getAddToCart(id:any):Observable<Model[]>{
    return this.http.get<Model[]>('https://localhost:7016/api/User/CardID'+id);
  }

}
