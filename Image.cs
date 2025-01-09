 File: Blob | null = null; // Use Blob for binary data

 <div style="margin-bottom: 1rem;">
    <label for="file">Upload File:</label>
    <input type="file" id="file" (change)="onFileSelected($event)" required />
  </div>

      onFileSelected(event: any) {
    const file = event.target.files[0];
    if (file) {
      const reader = new FileReader();
      reader.onload = () => {
        this.service.formData.File = reader.result as string;
      };
      reader.readAsDataURL(file);
    }
  }

import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../../../service/service.service';
import { Model } from '../../../service/model.model';

@Component({
  selector: 'app-seller-form',
  templateUrl: './seller-form.component.html',
  styleUrls: ['./seller-form.component.css'],
})
export class SellerFormComponent implements OnInit {
  base64Images: { [key: number]: string } = {}; // Store base64 strings with imageId as number
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

  // Convert image data to base64 format
  convertToBase64(imageData: any, imageId: number): void {
    const reader = new FileReader();
    reader.onload = () => {
      this.base64Images[imageId] = reader.result as string;
    };
    reader.onerror = (error) => {
      console.error('Error converting image to base64:', error);
    };
    
    // Create a Blob from imageData if it's not already a Blob or ArrayBuffer
    const blob = imageData instanceof Blob ? imageData : new Blob([imageData]);
    reader.readAsDataURL(blob); // Convert the Blob to base64
  }
}
