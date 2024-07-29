import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { Product } from 'src/Data/interfaces/product';

@Injectable({
  providedIn: 'root'
})
export class CartService {
  private cart: Product[] = [];
  private cartSubject = new BehaviorSubject<Product[]>([]);

  cart$ = this.cartSubject.asObservable();

  constructor() { }

  addToCart(product: Product) {
    const existingProduct = this.cart.find(item => item.id === product.id);
    
    if (existingProduct) {
      existingProduct.quantity += 1;
    } else {
      this.cart.push({ ...product, quantity: 1 });
    }
    this.cartSubject.next(this.cart);
  }

  getCartItems() {
    return this.cart;
  }

  updateCart(cart: Product[]) {
    this.cart = cart;
    this.cartSubject.next(this.cart);
  }
}
