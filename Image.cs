import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../../../service/service.service';
import { Model } from '../../../service/model.model';

@Component({
  selector: 'app-seller-form',
  templateUrl: './seller-form.component.html',
  styleUrls: ['./seller-form.component.css'], // Fixed typo from "styleUrl" to "styleUrls"
})
export class SellerFormComponent implements OnInit {
  base64Images: { [key: string]: string } = {}; // Store base64 strings for each image
  model: Model[] = []; // Hold the list of models

  constructor(public service: ServiceService) {}

  ngOnInit(): void {
    this.service.getImage().subscribe({
      next: (res) => {
        this.model = res as Model[]; // Cast the response to an array of Model
        console.log(res);

        // Convert each image's binary data to a base64 string
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

  // Function to convert binary image data to a base64 string
  convertToBase64(imageData: any, imageId: string): void {
    // Check if the imageData is already a Blob or ArrayBuffer
    if (imageData instanceof Blob || imageData instanceof ArrayBuffer) {
      const reader = new FileReader();
      reader.onload = () => {
        const base64String = reader.result as string;
        this.base64Images[imageId] = base64String; // Store the base64 string with the imageId
      };
      reader.onerror = (error) => {
        console.error('Error converting image to base64:', error);
      };

      reader.readAsDataURL(new Blob([imageData]));
    } else {
      console.warn('Invalid image data format for imageId:', imageId);
    }
  }
}

<div *ngFor="let img of model">
  <h3>{{ img.name }}</h3>
  <img *ngIf="base64Images[img.imageId]" [src]="base64Images[img.imageId]" alt="{{ img.name }}" />
</div>


<table>
  <tr *ngFor="let img of service.list">
    <td>{{ img.userId }}</td>
    <td>{{ img.roleId }}</td>
    <td>{{ img.imageId }}</td>
    <td>{{ img.imageName }}</td>
    <td>{{ img.imageDescription }}</td>
    <td>{{ img.price }}</td>
    <!-- Display the converted base64 image -->
    <td><img [src]="base64Images[img.imageId]" alt="{{ img.imageName }}" /></td>
  </tr>
</table>


