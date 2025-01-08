import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../../../service/service.service';
import { Model } from '../../../service/model.model';

@Component({
  selector: 'app-seller-form',
  standalone: false,
  templateUrl: './seller-form.component.html',
  styleUrls: ['./seller-form.component.css'] // Fixed typo from "styleUrl" to "styleUrls"
})
export class SellerFormComponent implements OnInit {
  base64Images: { [key: string]: string } = {}; // Store base64 strings
  model: any;

  constructor(public service: ServiceService) {}

  ngOnInit(): void {
    this.service.getImage().subscribe({
      next: (res) => {
        this.service.list = res as Model[];
        console.log(res);

        // Convert each image to base64
        this.service.list.forEach((img) => {
          this.convertToBase64(img.imageData, img.imageId);
        });
      },
      error: (err) => {
        console.error(err);
      },
    });
  }

  

  // Function to convert image byte data to base64
  convertToBase64(imageData: any, imageId: string): void {
    const byteArray = new Uint8Array(imageData);
    const binaryString = byteArray.reduce((data, byte) => data + String.fromCharCode(byte), '');
    const base64String = btoa(binaryString);

    // Store the base64 string with its respective ID
    this.base64Images[imageId] = `data:image/png;base64,${base64String}`;
  }
}
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


