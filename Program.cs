<h1>Hello Seller</h1>
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
                    <!-- <th>
                        <button class="btn btn-warning pt-2 mx-2" (click)="onEdit(ed)">Edit</button>
                        <button class="btn btn-danger mx-2" (click)="onDelete(ed.accountDetailId)">Delete</button>
                    </th> -->
                </tr>
            </tbody>
        </table>
    </div>

import { Component,OnInit, SecurityContext } from '@angular/core';
import { ServiceService } from '../../../service/service.service';
import { Model } from '../../../service/model.model';
import { NgFor,NgStyle } from '@angular/common';
import { DomSanitizer,SafeResourceUrl } from '@angular/platform-browser';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  base64Images: { [key: number]: string } = {}; 
 model: Model[] = []; 


 constructor(public service: ServiceService, private sanitizer:DomSanitizer) {}

 ngOnInit(): void {
   this.service.getImage().subscribe({
     next: (res) => {
       this.model = res as Model[]; 
       console.log(res);

      
       this.model.forEach((img) => {
         if (img.imageData) {
           this.convertToBase64(img.imageData, img.imageId);
         }
       });
     },
     error: (err) => {
       console.error('Error fetching images:', err);
     },
   });
 }


 convertToBase64(imageData: any, imageId: number): void {
   const reader = new FileReader();
   reader.onload = () => {
     this.base64Images[imageId] = reader.result as string;
   };
   reader.onerror = (error) => {
     console.error('Error converting image to base64:', error);
   };
   
   
   const blob = imageData instanceof Blob ? imageData : new Blob([imageData]);
   reader.readAsDataURL(blob); 
 }

}
