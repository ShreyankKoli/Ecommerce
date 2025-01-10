# Ecommerce
import { Component, OnInit } from '@angular/core';
import { ServiceService } from '../../../service/service.service';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-seller-form',
  templateUrl: './seller-form.component.html',
  styleUrls: ['./seller-form.component.css'],
})
export class SellerFormComponent implements OnInit {
  constructor(public service: ServiceService) {}

  ngOnInit(): void {}

  // Form submission
  onSubmit(form: NgForm): void {
    if (!this.service.formData.imageData) {
      alert('Please select an image to upload.');
      return;
    }

    this.service.postImage().subscribe({
      next: (res) => {
        console.log('Image uploaded successfully:', res);
        alert('Image uploaded successfully!');
        form.resetForm(); // Reset the form after submission
        this.service.formData = new Model(); // Clear formData
      },
      error: (err) => {
        console.error('Error uploading image:', err);
        alert('Image upload failed. Please try again.');
      },
    });
  }

  // File selection handler
  onFileSelected(event: any): void {
    const file = event.target.files[0];
    if (file) {
      this.service.formData.imageData = file;
    }
  }
}

<div class="container py-3">
  <div class="col-15">
    <div class="card">
      <div class="card-header bg-primary text-white">Product Details</div>
      <form #form="ngForm" (ngSubmit)="onSubmit(form)">
        <div class="row">
          <div class="col-11 mx-5">
            <label class="px-2 pt-2">Role ID</label>
            <input
              type="text"
              class="form-control border px-4 py-3 my-2 rounded-2"
              placeholder="Role ID"
              [(ngModel)]="service.formData.roleId"
              name="roleId"
              required
            />
          </div>

          <div class="col-11 mx-5">
            <label class="px-2 pt-2">User ID</label>
            <input
              type="text"
              class="form-control border px-4 py-3 my-2 rounded-2"
              placeholder="User ID"
              [(ngModel)]="service.formData.userId"
              name="userId"
              required
            />
          </div>

          <div class="col-11 mx-5">
            <label class="px-2 pt-2">Image Name</label>
            <input
              type="text"
              class="form-control border px-4 py-3 my-2 rounded-2"
              placeholder="Image Name"
              [(ngModel)]="service.formData.imageName"
              name="imageName"
              required
            />
          </div>

          <div class="col-11 mx-5">
            <label class="px-2 pt-2">Image Description</label>
            <input
              type="text"
              class="form-control border px-4 py-3 my-2 rounded-2"
              placeholder="Image Description"
              [(ngModel)]="service.formData.imageDescription"
              name="imageDescription"
            />
          </div>

          <div class="col-11 mx-5">
            <label class="px-2 pt-2">Price</label>
            <input
              type="text"
              class="form-control border px-4 py-3 my-2 rounded-2"
              placeholder="Price"
              [(ngModel)]="service.formData.price"
              name="price"
              required
            />
          </div>

          <div class="col-11 mx-5">
            <label class="px-2 pt-2">Image Data</label>
            <input
              type="file"
              class="form-control border px-4 py-3 my-2 rounded-2"
              (change)="onFileSelected($event)"
              name="imageData"
              required
            />
          </div>

          <div class="col-11 mx-5">
            <button class="btn btn-outline-primary mx-2 my-4" type="submit">
              Submit
            </button>
            <button class="btn btn-outline-secondary" type="reset">
              Reset
            </button>
          </div>
        </div>
      </form>
    </div>
  </div>
</div>

