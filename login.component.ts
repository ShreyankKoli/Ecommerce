import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {

  loginObj: any = {
    userName: '',
    password: ''
  };

  http = inject(HttpClient);

  constructor(private router: Router) {}

  onLogin() {
    // Prepare HTTP request body with user credentials
    const body = {
      userName: this.loginObj.userName,
      password: this.loginObj.password
    };

    // Token (typically a JWT token, but can be fetched from a service or environment)
    let token = 'your-jwt-token-here';  // Replace with actual token if needed

    // Set HTTP headers with Authorization token
    let headers = new HttpHeaders().set('Authorization', 'Bearer ' + token);

    // Make the POST request to the backend API to authenticate the user
    this.http.post('https://localhost:7016/api/User/Login', body, { headers }).subscribe({
      next: (res: any) => {
        console.log(res); // Log the response to check the structure

        if (res) {
          // Extract roleId from the response (based on the API response structure)
          const roleId = res.user?.roleId; // Access roleId from 'user' in response
          console.log("RoleId: ", roleId); // Check the roleId in the response

          if (roleId) {
            alert("Login Success");
            localStorage.setItem('login', this.loginObj.userName); // Store username in localStorage
            localStorage.setItem('roleId', roleId); // Store roleId in localStorage

            // Redirect based on roleId
            switch (roleId) {
              case 100:
                this.router.navigate(['/dashboard']); // Redirect to Admin Dashboard
                break;
              case 101:
                this.router.navigate(['/adminDashboard']); // Redirect to Admin Dashboard
                break;
              case 102:
                this.router.navigate(['/sellerDashboard']); // Redirect to Seller Dashboard
                break;
              default:
                alert("Contact Customer Support"); // Default case if no matching roleId
            }
          }
        } else {
          alert("Login Failed: " + res.message); // Handle failed login
        }
      },
      error: (err) => {
        console.error(err);
        alert("An error occurred: " + (err.error?.message || "Please try again later.")); // Error handling
      }
    });
  }
}
