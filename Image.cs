export class Model {
    imageId: number =  0
    roleId: number =  0
    imageName: string | null = null
    imageDescription: string | null = null
    price: string | null = null
    imageData: File | null = null;
    userId: number = 0 
}

import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment.development';
import { Mode } from 'fs';
import { Model } from './model.model';
import { HttpClient, HttpHeaders ,HttpErrorResponse } from '@angular/common/http';
import { catchError, Observable } from 'rxjs';


@Injectable({
  providedIn: 'root'
})
export class ServiceService {
  url: string = environment.apiBaseURL;
  list: Model[]=[];
  formData: Model = new Model();
  formsubmitted: boolean = false;
  handleError: any;

  constructor(private http: HttpClient) { }

  getImage():Observable<Model[]>{
    return this.http.get<Model[]>(this.url+'/get/all/image')
  }

  postImage():Observable<Model[]>{
    return this.http.post<Model[]>('https://localhost:7016/api/Image/upload',this.formData)
  }

}

<!-- <h1>Hello Seller</h1>
 <div class="row">
    <div class="col-12">
        <table class="table table-bordered table-secondary">
            <thead>
                <tr>
                    <th>userId</th>
                    <th>roleId</th>
                    <th>imageId</th>
                    <th>imageName</th>
                    <th>imageDescription</th>
                    <th>price</th>
                    <th>imageData</th>
                </tr>
            </thead>
            <tbody>
                <tr *ngFor="let img of service.list">
                    <th>{{img.userId}}</th>
                    <th>{{img.roleId}}</th>
                    <th>{{img.imageId}}</th>
                    <th>{{img.imageName}}</th>
                    <th>{{img.imageDescription}}</th>
                    <th>{{img.price}}</th>
                    <th><img [src]="base64Images[img.imageData]" alt="{{img.imageName}}"></th>
                    <th>{{img.imageData}}</th>
                    <th>
                        <button class="btn btn-warning pt-2 mx-2" (click)="onEdit(ed)">Edit</button>
                        <button class="btn btn-danger mx-2" (click)="onDelete(ed.accountDetailId)">Delete</button>
                    </th>
                </tr>
            </tbody>
        </table>
    </div> -->
<div class="container py-3">
    <div class="col-15">
        <div class="card">
            <div class="card-header bg-primary">
                Product Details
            </div>
            <form #form="ngForm" (submit)="onSubmit(form)">
                <div class="row">
                    <div class="col-11 mx-5">
                        <label class="px-2 pt-2">roleId</label>
                        <input type="text" class="form-control border px-4 py-3 my-2 rounded-2" placeholder="RoleId" #roleId="ngModel" [(ngModel)]="service.formData.roleId" name="roleId">
                    </div>

                    <div class="col-11 mx-5">
                        <label class="px-2 pt-2">userId</label>
                        <input type="text" class="form-control border px-4 py-3 my-2 rounded-2" placeholder="userId" #userId="ngModel" [(ngModel)]="service.formData.userId" name="userId">
                    </div>

                    <div class="col-11 mx-5">
                        <label class="px-2 pt-2">imageName</label>
                        <input type="text" class="form-control border px-4 py-3 my-2 rounded-2" placeholder="imageName" #imageName="ngModel" [(ngModel)]="service.formData.imageName" name="imageName">
                    </div>

                    <div class="col-11 mx-5">
                        <label class="px-2 pt-2">imageDescription</label>
                        <input type="text" class="form-control border px-4 py-3 my-2 rounded-2" placeholder="imageDescription" #imageDescription="ngModel" [(ngModel)]="service.formData.imageDescription" name="imageDescription">
                    </div>

                    <div class="col-11 mx-5">
                        <label class="px-2 pt-2">price</label>
                        <input type="text" class="form-control border px-4 py-3 my-2 rounded-2" placeholder="price" #price="ngModel" [(ngModel)]="service.formData.price" name="price">
                    </div>

                    <div class="col-11 mx-5">
                        <label class="px-2 pt-2">imageData</label>
                        <input type="file" class="form-control border px-4 py-3 my-2 rounded-2" placeholder="imageData" #imageData="ngModel" [(ngModel)]="service.formData.imageData" name="imageData">
                    </div>

                    

                        <div class="col-11 mx-4">
                            <!-- @if(service.formData.accountDetailId == 0){ -->
                            <button class="btn btn-outline-primary mx-2 my-4" type="submit">Submit</button>
                            <!-- } @else{
                            <button type="submit"  class="btn btn-outline-primary mx-2 my-4"
                                (click)="updateRecord(form)">Update</button>
                            } -->
                            <button class="btn btn-outline-secondary" type="reset">Reset</button>
                        </div>
                    </div>

                
            </form>
        </div>

    </div>
    <router-outlet></router-outlet>

                             import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../../../service/service.service';
import { URL } from 'url';
import { environment } from '../../../../environments/environment';
import { Model } from '../../../service/model.model';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-seller-form',
  standalone: false,

  templateUrl: './seller-form.component.html',
  styleUrl: './seller-form.component.css'
})
export class SellerFormComponent implements OnInit {

  constructor(public service: ServiceService) { }

  onSubmit(form:NgForm){
    this.service.postImage()
    .subscribe({
      next: res=>{
        this.service.list = res as Model[];
      },
      error: err=>{
        console.error(err);
      }
    })
  }





  ngOnInit(): void {
  //   this.service.getImage()
  //     .subscribe({
  //       next: res => {
  //         this.service.list = res as Model[];
  //         console.log(res);
  //         this.service.list.forEach((img) => {
  //           this.convertToBase64(img.imageData, img.imageId);
  //         });
  //       },
  //       error: err => {
  //         console.error(err);
  //       }
  //     })
  // }

  // convertToBase64(imageData: any, imageId: number): void {
  //   const byteArray = new Uint8Array(imageData);
  //   console.log(byteArray);
  //   const binaryString = byteArray.reduce((data, byte) => data + String.fromCharCode(byte), '');
  //   const base64String = btoa(binaryString);
  //   this.base64Images[imageId] = `data:image/png;base64,${base64String}`;
  }
 }
  




