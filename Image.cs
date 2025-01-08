import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../../../service/service.service';
import { URL } from 'url';
import { environment } from '../../../../environments/environment';
import { Model } from '../../../service/model.model';

@Component({
  selector: 'app-seller-form',
  standalone: false,

  templateUrl: './seller-form.component.html',
  styleUrl: './seller-form.component.css'
})
export class SellerFormComponent implements OnInit {
  base64: any;
model: any;

  constructor(public service: ServiceService) { }

  ngOnInit(): void {
    this.service.getImage()
      .subscribe({
        next: res => {
          this.service.list = res as Model[];
          console.log(res);
        },
        error: err => {
          console.error(err);
        }
      })

  }


}
<tr *ngFor="let img of service.list">
                    <th>{{img.userId}}</th>
                    <th>{{img.roleId}}</th>
                    <th>{{img.imageId}}</th>
                    <th>{{img.imageName}}</th>
                    <th>{{img.imageDescription}}</th>
                    <th>{{img.price}}</th>
                    <th><img [src]="img.imageData"></th>
                    <th>{{img.imageData}}</th>
