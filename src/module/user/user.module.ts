import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { UserRoutingModule } from './user-routing.module';
import { ShopAndCartComponent } from './Componantes/stateful/shop-and-cart/shop-and-cart.component';
import { ProductsComponent } from './Componantes/stateful/products/products.component';
import { CartComponent } from './Componantes/stateful/cart/cart.component';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '../material/material.module';


@NgModule({
  declarations: [
    ShopAndCartComponent,
    ProductsComponent,
    CartComponent
  ],
  imports: [
    CommonModule,
    UserRoutingModule,
    FormsModule,
    MaterialModule
  ]
})
export class UserModule { }
