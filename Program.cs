import { Component } from '@angular/core';
import { ServiceService } from '../../../service/service.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent {
  selectedFile: File | null = null;

  constructor(private service: ServiceService) {}

  onFileSelected(event: any): void {
    this.selectedFile = event.target.files[0];
  }

  uploadImage(): void {
    if (!this.selectedFile) {
      console.error('No file selected!');
      return;
    }

    const reader = new FileReader();
    reader.onload = () => {
      const base64Image = reader.result;
      const imageData = {
        userId: 1, // Replace with actual userId
        roleId: 2, // Replace with actual roleId
        imageName: this.selectedFile?.name,
        imageDescription: 'Sample image description', // Add your description
        price: 100, // Replace with actual price
        imageData: base64Image,
      };

      this.service.postImage(imageData).subscribe({
        next: (res) => {
          console.log('Image uploaded successfully:', res);
        },
        error: (err) => {
          console.error('Error uploading image:', err);
        },
      });
    };

    reader.readAsDataURL(this.selectedFile);
  }
}

<h1>Upload Image</h1>

<div class="row mt-4">
  <div class="col-6">
    <input type="file" (change)="onFileSelected($event)" class="form-control" />
  </div>
  <div class="col-6">
    <button class="btn btn-primary" (click)="uploadImage()">Upload Image</button>
  </div>
</div>



