import { Component, OnInit, SimpleChanges } from '@angular/core';
import { AuthService } from '../../../../services/Auth/auth.service';
import { Router } from '@angular/router';
import { BehaviorSubject, Subscription } from 'rxjs';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  userRole:any="";
  isAuthenticated: boolean = false;
  subscriptionAuthentication !:Subscription;

  constructor(private authService:AuthService,private router:Router) { }

  ngOnInit(): void 
  {
    this.updateAuthStatus();

    this.subscriptionAuthentication = this.authService.getAuthStatus().subscribe(status => {
      this.updateAuthStatus();
    });
  }

  private updateAuthStatus() 
  {
    this.isAuthenticated = this.authService.isAuthenticated();
    const decodedToken = this.authService.getDecodedToken();
    this.userRole = decodedToken ? decodedToken : "";
    console.log(this.isAuthenticated, this.userRole);
  }

  LogOut() {
    this.authService.logout();
    this.router.navigate(['/Login']);
  }

  ngOnDestroy(): void {
    if (this.subscriptionAuthentication) {
      this.subscriptionAuthentication.unsubscribe();
    }
  }
}
