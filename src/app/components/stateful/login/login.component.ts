import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/services/Auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  email: string = '';
  password: string = '';

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
  }


  onSubmit() {
    this.authService.login(this.email, this.password).subscribe({
      next:(res)=>{
        console.log(res);
      if (res == true )
      {
        this.router.navigate(["/Home"])
      }
      },
      error:(err)=>{console.log(err);}
    });
  }
}


