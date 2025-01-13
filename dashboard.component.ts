import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../../../service/service.service';
import { Model } from '../../../service/model.model';
import { Router } from '@angular/router';

@Component({
  selector: 'app-dashboard',
  standalone: false,
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent implements OnInit {
  model: Model[] = [];
  cartCount: number = 0;

  constructor(private service: ServiceService, public router: Router) {}

  ngOnInit(): void {
    this.fetchImages();
  }

  fetchImages(): void {
    this.service.getImage().subscribe({
      next: (res) => {
        this.model = res as Model[];
        console.log(res);
      },
      error: (err) => {
        console.error('Error fetching images:', err);
      }
    });
  }

  addCart(imageId: number): void {
    const localData = localStorage.getItem('login');
    const userData = localData ? JSON.parse(localData) : null;
    const userId = userData?.id || 0;

    if (userId === 0) {
      alert('User not logged in. Please log in to add items to the cart.');
      this.router.navigate(['/login']);
      return;
    }

    const cartObj = {
      cartId: 0,
      imageId: imageId,
      roleId: 102,
      userId: userId,
      quantity: 1
    };

    this.service.addToCart(cartObj).subscribe({
      next: (res: any) => {
        alert('Item added to cart successfully!');
        this.service.sharedData(cartObj);
        this.router.navigate(['/cart']);
      },
      error: (err: any) => {
        console.error('Error adding item to cart:', err);
        alert('Failed to add item to cart. Please try again later.');
      }
    });
  }
}

<div *ngFor="let item of model">
  <div>
    <h3>{{ item.name }}</h3>
    <button (click)="addCart(item.imageId)">Add to Cart</button>
  </div>
</div>


