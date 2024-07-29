import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { TokenInterceptor } from './token.interceptor';
import { HTTP_INTERCEPTORS, HttpClient } from '@angular/common/http';
import { AuthService } from 'src/services/Auth/auth.service';
import { environment } from 'src/environments/environment';
import { Product } from '../../Data/interfaces/product';

describe('TokenInterceptor', () => {
  let httpTestingController: HttpTestingController;
  let httpClient: HttpClient;
  let authService: jasmine.SpyObj<AuthService>;
  let basURL=environment.apiUrl;
  beforeEach(() => {
    const authServiceSpy = jasmine.createSpyObj('AuthService', ['getToken']);
    var basURL=environment.apiUrl;
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [
        { provide: AuthService, useValue: authServiceSpy },
        TokenInterceptor,
        {
          provide: HTTP_INTERCEPTORS,
          useClass: TokenInterceptor,
          multi: true
        }
      ]
    });

    httpTestingController = TestBed.inject(HttpTestingController);
    httpClient = TestBed.inject(HttpClient);
    authService = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
  });

  afterEach(() => {
    httpTestingController.verify();
  });

  it('should add Authorization header for non-auth requests', () => {
    // Arrange
    const token = 'fake-token';
    authService.getToken.and.returnValue(token);

    // Act
    httpClient.get(basURL+"/Product").subscribe();

    // Assert
    const req = httpTestingController.expectOne(basURL+"/Product");
    expect(req.request.headers.has('Authorization')).toBeTrue();
    expect(req.request.headers.get('Authorization')).toBe(`Bearer ${token}`);
    req.flush({});
  });

  it('should not add Authorization header for auth requests', () => {
    // Arrange
    const token = 'fake-token';
    authService.getToken.and.returnValue(token);

    // Act
    httpClient.get(basURL+'/Account/login').subscribe();

    // Assert
    const req = httpTestingController.expectOne(basURL+'/Account/login');
    expect(req.request.headers.has('Authorization')).toBeFalse();
    req.flush({});
  });

})
