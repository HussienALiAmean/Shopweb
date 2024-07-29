import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, catchError, map, observable, of } from 'rxjs';
import { environment } from 'src/environments/environment';
import  { jwtDecode }  from "jwt-decode";
import { JwtPayload } from 'src/Data/interfaces/jwt-payload';
import { Observable } from 'rxjs/internal/Observable';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  
  private authStatus = new BehaviorSubject<boolean>(this.isAuthenticated() );

  private tokenKey = 'authToken';
  private decodedToken: any;
  private apiUrl = environment.apiUrl+"/Account"; // Replace with your API URL

  constructor(private http: HttpClient) 
  {  }

  login(email: string, password: string): Observable<boolean> 
  {
    return this.http.post<any>(`${this.apiUrl}/login`, { Email:email, Password:password }).pipe(
      map(response => {
        // Handle successful login
        this.setToken(response.token);
        return true;
      }),
      catchError(error => {
        // Handle login error
        console.error('Login error', error);
        return of(false);
      })
    );
  }

  register(email: string, password: string): Observable<boolean> {
    const body = { Email:email, Password:password , role:"User" };
    return this.http.post<boolean>(`${this.apiUrl}/register`, body).pipe(
      map(() => true),
      catchError(() => of(false))
    );
  }


  getToken() {
    return localStorage.getItem(this.tokenKey);
  }

  setToken(token: string) {
    localStorage.setItem(this.tokenKey, token);
    this.authStatus.next(true);
  }

  getDecodedToken() {
    const token = this.getToken();
    return token ? jwtDecode<JwtPayload>(token).Roles : null;
  }

  getCurrentUserRoles() {
    const decodedToken = this.getDecodedToken();
    return decodedToken ? decodedToken : null;
  }

  isAuthenticated() {
    return !!this.getToken();
  }

  getAuthStatus(): Observable<boolean> {
    return this.authStatus.asObservable();
  }


  logout() {
    localStorage.removeItem(this.tokenKey);
    this.authStatus.next(false);
  }

}
