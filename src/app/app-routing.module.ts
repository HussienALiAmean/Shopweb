import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/stateful/home/home.component';
import { LoginComponent } from './components/stateful/login/login.component';
import { RegisterComponent } from './components/stateful/register/register.component';
import { NotFoundComponent } from './components/stateful/not-found/not-found.component';
import { AuthGuard } from 'src/guards/auth/auth.guard';

const routes: Routes = [ 
  { path: '', redirectTo:'', pathMatch:'full'},

  { path: 'Home', component: HomeComponent },

  { path: 'Admin', 
    loadChildren: () => import('../module/admin/admin.module').then(m => m.AdminModule),
    canActivate: [AuthGuard],
    data: { requiredRole: 'Admin' }} ,

  { path: 'ShopAndCart',
    loadChildren: () => import('../module/user/user.module').then(m => m.UserModule),
    canActivate: [AuthGuard],
    data: { requiredRole: 'User' }},

  { path: 'Login', component: LoginComponent },

  { path: 'Register', component: RegisterComponent },

  { path: '**', component:NotFoundComponent } ]
@NgModule({
  imports: [RouterModule.forRoot(routes,{useHash:true})],
  exports: [RouterModule]
})
export class AppRoutingModule { }
