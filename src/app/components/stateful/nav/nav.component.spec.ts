import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NavComponent } from './nav.component';
import { AuthService } from 'src/services/Auth/auth.service';
import { Router } from '@angular/router';
import { of } from 'rxjs';

describe('NavComponent', () => {
  let component: NavComponent;
  let fixture: ComponentFixture<NavComponent>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let routerSpy: jasmine.SpyObj<Router>;

  beforeEach(async () => {
    const authServiceMock = jasmine.createSpyObj('AuthService', ['getAuthStatus', 'isAuthenticated', 'getDecodedToken', 'logout']);
    const routerMock = jasmine.createSpyObj('Router', ['navigate']);

    await TestBed.configureTestingModule({
      declarations: [NavComponent],
      providers: [
        { provide: AuthService, useValue: authServiceMock },
        { provide: Router, useValue: routerMock }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(NavComponent);
    component = fixture.componentInstance;
    authServiceSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    routerSpy = TestBed.inject(Router) as jasmine.SpyObj<Router>;

    // Mock implementations
    authServiceSpy.getAuthStatus.and.returnValue(of(true));
    authServiceSpy.isAuthenticated.and.returnValue(true);
    authServiceSpy.getDecodedToken.and.returnValue('Admin');

    fixture.detectChanges(); // Trigger initial data binding
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize authentication status and user role on init', () => {
    component.ngOnInit();

    expect(component.isAuthenticated).toBeTrue();
    expect(component.userRole).toBe('Admin');
    expect(authServiceSpy.getAuthStatus).toHaveBeenCalled();
  });

  it('should update authentication status and user role when auth status changes', () => {
    authServiceSpy.isAuthenticated.and.returnValue(false);
    authServiceSpy.getDecodedToken.and.returnValue('User' );

    component.ngOnInit();

    expect(component.isAuthenticated).toBeFalse();
    expect(component.userRole).toBe('User');
  });

  it('should log out and navigate to login page', () => {
    component.LogOut();

    expect(authServiceSpy.logout).toHaveBeenCalled();
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/Login']);
  });

  it('should unsubscribe on destroy', () => {
    spyOn(component.subscriptionAuthentication, 'unsubscribe');

    component.ngOnDestroy();

    expect(component.subscriptionAuthentication.unsubscribe).toHaveBeenCalled();
  });
});