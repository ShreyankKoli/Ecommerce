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
        <tr *ngFor="let img of model">
          <td>{{ img.userId }}</td>
          <td>{{ img.roleId }}</td>
          <td>{{ img.imageId }}</td>
          <td>{{ img.imageName }}</td>
          <td>{{ img.imageDescription }}</td>
          <td>{{ img.price }}</td>
          <td>
            <img *ngIf="base64Images[img.imageId]" [src]="base64Images[img.imageId]" alt="{{ img.imageName }}" style="width: 100px; height: auto;">
          </td>
        </tr>
      </tbody>
    </table>
  </div>
</div>

import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../../../service/service.service';
import { Model } from '../../../service/model.model';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css'],
})
export class DashboardComponent implements OnInit {
  base64Images: { [key: number]: string } = {};
  model: Model[] = [];

  constructor(private service: ServiceService) {}

  ngOnInit(): void {
    this.fetchImages();
  }

  fetchImages(): void {
    this.service.getImage().subscribe({
      next: (res) => {
        this.model = res as Model[];
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

    // Ensure the input is a Blob before reading
    const blob = imageData instanceof Blob ? imageData : new Blob([imageData]);
    reader.readAsDataURL(blob);
  }
}

