import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/stateful/home/home.component';
import { LoginComponent } from './components/stateful/login/login.component';
import { RegisterComponent } from './components/stateful/register/register.component';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { ProductService } from 'src/services/Product/product.service';
import { OrderService } from 'src/services/Order/order.service';
import { NavComponent } from './components/stateful/nav/nav.component';
import { NotFoundComponent } from './components/stateful/not-found/not-found.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { MaterialModule } from 'src/module/material/material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AuthService } from 'src/services/Auth/auth.service';
import { TokenInterceptor } from 'src/Interceptors/token/token.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    RegisterComponent,
    NavComponent,
    NotFoundComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule ,
    MaterialModule,
    FormsModule ,
    ReactiveFormsModule
  ],
  providers: [
    OrderService,
    ProductService,
    AuthService,
    { provide: HTTP_INTERCEPTORS, useClass: TokenInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
