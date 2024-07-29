import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from 'src/services/Auth/auth.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private authService: AuthService, private router: Router) {}


  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {

      const requiredRole = route.data['requiredRole'];
      const currentUserRoles = this.authService.getCurrentUserRoles(); // Adjust this method to get the current user
      const currentUser = this.authService.isAuthenticated(); // Adjust this method to get the current user
     
      console.log(currentUser);

      if (currentUser && currentUserRoles === requiredRole) {
        return true;
      }

      // Navigate to login page or show an error message
      this.router.navigate(['/Login']);
      return false;


  }
  
}
