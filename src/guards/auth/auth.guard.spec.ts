import { TestBed } from '@angular/core/testing';
import { AuthGuard } from '../auth/auth.guard';
import { AuthService } from 'src/services/Auth/auth.service';
import { Router } from '@angular/router';

describe('AuthGuard', () => {
  let guard: AuthGuard;
  let authService: jasmine.SpyObj<AuthService>;
  let router: jasmine.SpyObj<Router>;

  beforeEach(() => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['isAuthenticated', 'getCurrentUserRoles']);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    TestBed.configureTestingModule({
      providers: [
        AuthGuard,
        { provide: AuthService, useValue: authServiceSpy },
        { provide: Router, useValue: routerSpy }
      ]
    });

    guard = TestBed.inject(AuthGuard);
    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    router = TestBed.inject(Router) as jasmine.SpyObj<Router>;
  });

  it('should allow navigation if user is authenticated and has the required role', () => {
    // Arrange
    const requiredRole = 'Admin';
    const currentUserRoles = 'Admin';
    const currentUser = true; // User is authenticated
    authService.isAuthenticated.and.returnValue(currentUser);
    authService.getCurrentUserRoles.and.returnValue(currentUserRoles);

    const route: any = { data: { requiredRole } };
    const state: any = {};

    // Act
    const result = guard.canActivate(route, state);

    // Assert
    expect(result).toBeTrue();
    expect(router.navigate).not.toHaveBeenCalled();
  });

  it('should deny navigation if user is not authenticated', () => {
    // Arrange
    const requiredRole = 'Admin';
    const currentUserRoles = 'User';
    const currentUser = false; // User is not authenticated
    authService.isAuthenticated.and.returnValue(currentUser);
    authService.getCurrentUserRoles.and.returnValue(currentUserRoles);

    const route: any = { data: { requiredRole } };
    const state: any = {};

    // Act
    const result = guard.canActivate(route, state);

    // Assert
    expect(result).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(['/Login']);
  });

  it('should deny navigation if user does not have the required role', () => {
    // Arrange
    const requiredRole = 'Admin';
    const currentUserRoles = 'User';
    const currentUser = true; // User is authenticated
    authService.isAuthenticated.and.returnValue(currentUser);
    authService.getCurrentUserRoles.and.returnValue(currentUserRoles);

    const route: any = { data: { requiredRole } };
    const state: any = {};

    // Act
    const result = guard.canActivate(route, state);

    // Assert
    expect(result).toBeFalse();
    expect(router.navigate).toHaveBeenCalledWith(['/Login']);
  });
});