import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ShopAndCartComponent } from './Componantes/stateful/shop-and-cart/shop-and-cart.component';

const routes: Routes = [
  { path: '', component: ShopAndCartComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class UserRoutingModule { }
