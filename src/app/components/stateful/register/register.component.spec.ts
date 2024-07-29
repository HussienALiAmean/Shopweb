import { ComponentFixture, TestBed } from '@angular/core/testing';

import { RegisterComponent } from './register.component';
import { Router } from '@angular/router';
import { AuthService } from 'src/services/Auth/auth.service';
import { ReactiveFormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';

// Mock AuthService
class AuthServiceMock {
  register = jasmine.createSpy('register').and.returnValue(of(true));
}

// Mock Router
class RouterMock {
  navigate = jasmine.createSpy('navigate');
}

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let authServiceMock: AuthServiceMock;
  let routerMock: RouterMock;

  beforeEach(async () => {
    authServiceMock = new AuthServiceMock();
    routerMock = new RouterMock();

    await TestBed.configureTestingModule({
      declarations: [ RegisterComponent ],
      imports: [ ReactiveFormsModule ],
      providers: [
        { provide: AuthService, useValue: authServiceMock },
        { provide: Router, useValue: routerMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

 

  it('should return appropriate error message for email control', () => {
    const emailControl = component.registrationForm.get('email');
    
    emailControl?.setValue('');
    expect(component.getEmailErrorMessage()).toBe('Email is required');
    
    emailControl?.setValue('invalid-email');
    expect(component.getEmailErrorMessage()).toBe('Enter a valid email');
  });

  it('should validate password match', () => {
    component.registrationForm.get('password')?.setValue('password123');
    component.registrationForm.get('confirmPassword')?.setValue('password123');
    expect(component.registrationForm.get('confirmPassword')?.valid).toBeTrue();

    component.registrationForm.get('confirmPassword')?.setValue('password124');
    expect(component.registrationForm.get('confirmPassword')?.hasError('mismatch')).toBeTrue();
  });

  it('should call AuthService.register on form submit with valid data', () => {
    const form = component.registrationForm;
    form.controls['email'].setValue('test@example.com');
    form.controls['password'].setValue('password123');
    form.controls['confirmPassword'].setValue('password123');

    component.register();

    expect(authServiceMock.register).toHaveBeenCalledWith('test@example.com', 'password123');
  });

  it('should navigate to /Login on successful registration', () => {
    authServiceMock.register.and.returnValue(of(true));

    const form = component.registrationForm;
    form.controls['email'].setValue('test@example.com');
    form.controls['password'].setValue('password123');
    form.controls['confirmPassword'].setValue('password123');

    component.register();

    expect(routerMock.navigate).toHaveBeenCalledWith(['/Login']);
  });

  it('should show alert on registration error', () => {
    authServiceMock.register.and.returnValue(of(false));
    spyOn(window, 'alert'); // Spy on window.alert

    const form = component.registrationForm;
    form.controls['email'].setValue('test@example.com');
    form.controls['password'].setValue('password123');
    form.controls['confirmPassword'].setValue('password123');

    component.register();

    expect(window.alert).toHaveBeenCalledWith("Registration Error");
  });

  it('should handle registration error', () => {
    const errorResponse = { message: 'Registration failed' };
    authServiceMock.register.and.returnValue(throwError(() => errorResponse));
    spyOn(console, 'error'); // Spy on console.error

    const form = component.registrationForm;
    form.controls['email'].setValue('test@example.com');
    form.controls['password'].setValue('password123');
    form.controls['confirmPassword'].setValue('password123');

    component.register();

    expect(console.error).toHaveBeenCalledWith('Registration failed', errorResponse);
  });
});