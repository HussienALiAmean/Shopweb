import { Component, OnInit } from '@angular/core';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/services/Auth/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  registrationForm: FormGroup;


  constructor(private fb: FormBuilder , private authService: AuthService, private router:Router) {
    this.registrationForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
      confirmPassword: ['', [Validators.required, this.matchPassword.bind(this)]]
    });
  }
  ngOnInit(): void {
  }

  

  getEmailErrorMessage() {
    if (this.registrationForm.get('email')?.hasError('required')) {
      return 'Email is required';
    }
    return this.registrationForm.get('email')?.hasError('email') ? 'Enter a valid email' : '';
  }

  matchPassword(control: AbstractControl): { [key: string]: boolean } | null {
    if (this.registrationForm) {
      return control.value === this.registrationForm.get('password')?.value ? null : { mismatch: true };
    }
    return null;
  }

  register() {
    if (this.registrationForm.valid) {
      const { email, password } = this.registrationForm.value;
      this.authService.register(email, password).subscribe({
        next: (response) => {
          if(response==true)
          {
            console.log('Registration successful', response);
            alert("Registration successful");
            this.router.navigate(['/Login']);
          }
          else
          {
            console.log('Registration Error');
            alert("Registration Error");
          }
        },
        error: (error) => {
          console.error('Registration failed', error);
          // Handle registration error logic
        }
      });
    }
  }
}