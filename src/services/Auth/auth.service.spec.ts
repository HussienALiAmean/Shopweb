import { TestBed } from '@angular/core/testing';

import { AuthService } from './auth.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { JwtPayload } from 'jwt-decode';
import { environment } from 'src/environments/environment';

describe('AuthService', () => {

let service: AuthService;
let httpMock: HttpTestingController;

const Token = 'mockToken';
const DecodedToken:string = 'user' ;

beforeEach(() => {
  TestBed.configureTestingModule({
    imports: [HttpClientTestingModule],
    providers: [AuthService]
  });

  service = TestBed.inject(AuthService);
  httpMock = TestBed.inject(HttpTestingController);
  localStorage.removeItem('authToken'); // Clear any existing tokens
});

afterEach(() => {
  httpMock.verify();
});

it('should be created', () => {
  expect(service).toBeTruthy();
});

describe('login', () => {
  it('should set token on successful login', () => {
    const email = 'test@example.com';
    const password = 'password';
    const response = { token: Token };

    service.login(email, password).subscribe(result => {
      expect(result).toBeTrue();
      expect(localStorage.getItem('authToken')).toBe(Token);
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/Account/login`);
    expect(req.request.method).toBe('POST');
    req.flush(response);
  });

  it('should return false on login failure', () => {
    const email = 'test@example.com';
    const password = 'password';

    service.login(email, password).subscribe(result => {
      expect(result).toBeFalse();
      expect(localStorage.getItem('authToken')).toBeNull();
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/Account/login`);
    expect(req.request.method).toBe('POST');
    req.flush({}, { status: 400, statusText: 'Bad Request' });
  });
});

describe('register', () => {
  it('should return true on successful registration', () => {
    const email = 'test@example.com';
    const password = 'password';

    service.register(email, password).subscribe(result => {
      expect(result).toBeTrue();
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/Account/register`);
    expect(req.request.method).toBe('POST');
    req.flush(true);
  });

  it('should return false on registration failure', () => {
    const email = 'test@example.com';
    const password = 'password';

    service.register(email, password).subscribe(result => {
      expect(result).toBeFalse();
    });

    const req = httpMock.expectOne(`${environment.apiUrl}/Account/register`);
    expect(req.request.method).toBe('POST');
    req.flush({}, { status: 400, statusText: 'Bad Request' });
  });
});

describe('token management', () => {
  it('should set and get token correctly', () => {
    service.setToken(Token);
    expect(localStorage.getItem('authToken')).toBe(Token);

    const token = service.getToken();
    expect(token).toBe(Token);
  });

  it('should remove token on logout', () => {
    service.setToken(Token);
    expect(localStorage.getItem('authToken')).toBe(Token);

    service.logout();
    expect(localStorage.getItem('authToken')).toBeNull();
  });
});

describe('authentication status', () => {
  it('should return true if token exists', () => {
    service.setToken(Token);
    expect(service.isAuthenticated()).toBeTrue();
  });

  it('should return false if token does not exist', () => {
    service.logout();
    expect(service.isAuthenticated()).toBeFalse();
  });

  it('should get current user roles from token', () => {
    spyOn(service, 'getToken').and.returnValue(Token);
    spyOn<any>(service, 'getDecodedToken').and.returnValue('User');

    const roles = service.getCurrentUserRoles();
    expect(roles).toEqual('User');
  });
});
});