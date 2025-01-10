import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ImageService {
  private apiUrl = 'https://your-api-url/api/image/upload'; // Replace with your API URL

  constructor(private http: HttpClient) {}

  uploadImage(file: File, roleId: number, imageName: string): Observable<any> {
    const formData = new FormData();
    formData.append('File', file); // 'File' matches the API's DTO property name
    formData.append('RoleId', roleId.toString());
    formData.append('ImageName', imageName);

    return this.http.post(this.apiUrl, formData);
  }
}

import { Component } from '@angular/core';
import { ImageService } from './image.service'; // Adjust path if needed

@Component({
  selector: 'app-image-upload',
  templateUrl: './image-upload.component.html',
  styleUrls: ['./image-upload.component.css'],
})
export class ImageUploadComponent {
  selectedFile: File | null = null;
  imageName: string = '';
  roleId: number = 0;
  uploadMessage: string = '';

  constructor(private imageService: ImageService) {}

  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.selectedFile = file;
    }
  }

  onUpload(): void {
    if (!this.selectedFile || !this.imageName || this.roleId <= 0) {
      this.uploadMessage = 'Please fill all the fields and select a file.';
      return;
    }

    this.imageService
      .uploadImage(this.selectedFile, this.roleId, this.imageName)
      .subscribe(
        (response) => {
          this.uploadMessage = 'Image uploaded successfully.';
          console.log(response);
        },
        (error) => {
          this.uploadMessage = 'Error uploading image.';
          console.error(error);
        }
      );
  }
}


<div class="image-upload-container">
  <h2>Upload Image</h2>
  <form (submit)="onUpload()">
    <div>
      <label for="roleId">Role ID:</label>
      <input
        type="number"
        id="roleId"
        [(ngModel)]="roleId"
        name="roleId"
        required
      />
    </div>
    <div>
      <label for="imageName">Image Name:</label>
      <input
        type="text"
        id="imageName"
        [(ngModel)]="imageName"
        name="imageName"
        required
      />
    </div>
    <div>
      <label for="file">Choose File:</label>
      <input type="file" id="file" (change)="onFileSelected($event)" />
    </div>
    <button type="button" (click)="onUpload()">Upload</button>
  </form>
  <p>{{ uploadMessage }}</p>
</div>




