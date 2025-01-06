import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Component, inject } from '@angular/core';
import {FormsModule} from '@angular/forms';
import { Router } from '@angular/router';



@Component({
  selector: 'app-login',
  standalone: false,
  
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {

  loginObj: any = {
    "userName": "",
    "password": ""
  };

  http=inject(HttpClient);

  constructor(private router:Router){
   
  }
  

  onLogin() {
    const params = new HttpParams()   //HttpParams used to request certain resources from web server.
    .set('userName',this.loginObj.userName)
    .set('password',this.loginObj.password);

    let Token ='eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJKd3RTdWJqZWN0IiwianRpIjoiN2Y0ZDYwYjMtODA0NS00YWFlLTk0ZGQtOGJmMzJiM2M2MDNhIiwidXNlck5hbWUiOiJrYXlib2x1IiwiRmlyc3ROYW1lIjoia2F5bmFrbyIsImV4cCI6MTczNjE2NDg2MywiaXNzIjoiSnd0SXNzdWVyIiwiYXVkIjoiSnd0QXVkaWVuY2UifQ.NsfO61Mz8v8Cb3wilDO0Lj0RNGlRWWgzeaLeAVi53rs';
    let head_obj = new HttpHeaders().set('Authorization','bearer'+Token);
    this.http.get("https://localhost:7016/api/User/Login",{headers:head_obj , params})
      .subscribe({
        next: (res: any) => {
          console.log(res);
          if (res) {  
            // alert("Login Success");
            if(typeof window !== 'undefined' && window.localStorage)
            {
          console.log(res);
              localStorage.setItem("login",this.loginObj.userName);
            }

            const roldeId = res.data?.roleId || res.roleId;
            if(roldeId){
              alert("Login Success");
            localStorage.setItem("roleId",roldeId)
             //this.router.navigate(['/dashboard']);
            
              switch(roldeId){
                case 100:
                  this.router.navigate(['/dashboard']);
                  break;
                case 101:
                    this.router.navigate(['/adminDashboard']);
                    break;
                case 102:
                  this.router.navigate(['/sellerDashboard']);
                  break;
                 default:
                  alert("Contact Customer Support"); 
              }
             }
          } else {
            alert("Login Failed: " + res.message);
          }
        },
        error: (err) => {
          console.error(err);
          alert("An error occurred: " + (err.error?.message || "Please try again later."));
        }
      });
  }
}

