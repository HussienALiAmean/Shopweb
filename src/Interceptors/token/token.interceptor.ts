import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable } from 'rxjs';
import { AuthService } from 'src/services/Auth/auth.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

  constructor(private authService: AuthService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
      // Skip the auth endpoint
      if (request.url.includes('Account')) {
        return next.handle(request);
      }

        // Get the token from the auth service
        const token = this.authService.getToken();

        // Clone the request and add the Authorization header
        const cloned = request.clone({
          headers: request.headers.set('Authorization', `Bearer ${token}`)
        });
        
        console.log(cloned);
        // Pass the cloned request instead of the original request to the next handler
        return next.handle(cloned);
        
      }
    }