import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoginComponent } from './login.component';
import { AuthService } from 'src/services/Auth/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';

describe('LoginComponent', () => {
  let component: LoginComponent;
  let fixture: ComponentFixture<LoginComponent>;
  let authService: jasmine.SpyObj<AuthService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['login']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [LoginComponent],
      imports: [FormsModule], 
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(LoginComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should call AuthService login on form submit', () => {
    // Arrange
    component.email = 'A12@gmail.com';
    component.password = '111@qQ22';
    authService.login.and.returnValue(of(true)); // Mock successful login

    // Act
    component.onSubmit();

    // Assert
    expect(authService.login).toHaveBeenCalledWith('A12@gmail.com', '111@qQ22');
  });

  it('should navigate to /Home on successful login', () => {
    // Arrange
    authService.login.and.returnValue(of(true));

    // Act
    component.onSubmit();

    // Assert
    expect(router.navigate).toHaveBeenCalledWith(['/Home']);
  });

  it('should not navigate on failed login', () => {
    // Arrange
    authService.login.and.returnValue(of(false));

    // Act
    component.onSubmit();

    // Assert
    expect(router.navigate).not.toHaveBeenCalled();
  });

  it('should handle login error', () => {
    // Arrange
    const errorResponse = { message: 'Login failed' };
    authService.login.and.returnValue(throwError(errorResponse));
    spyOn(console, 'log'); // Spy on console.log

    // Act
    component.onSubmit();

    // Assert
    expect(console.log).toHaveBeenCalledWith(errorResponse);
  });
});