import { Component, OnInit } from '@angular/core';
import { Product } from 'src/Data/interfaces/product';
import { CartService } from 'src/services/Cart/cart.service';
import { OrderService } from '../../../../../services/Order/order.service';
import { Order } from '../../../../../Data/interfaces/order';
import { OrderProduct } from 'src/Data/interfaces/order-product';
import { AuthService } from '../../../../../services/Auth/auth.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit {

  cart: Product[] = [];

  constructor(private cartService: CartService ,private orderService:OrderService,private authService:AuthService) { }

  ngOnInit(): void {
    this.cartService.cart$.subscribe(cart => {
      this.cart = cart;
    });
  }

  updateQuantity(product: Product, quantity: number) {
    product.quantity = quantity;
    this.cartService.updateCart(this.cart);
  }

  getTotalPrice() {
    return this.cart.reduce((total, product) => total + (product.price * product.quantity), 0);
  }

  submitOrder() {
    if (this.cart.length > 0) {
      const orderProducts: OrderProduct[] = this.cart.map(product => ({
        ProductId: product.id,
        Quantity: product.quantity,
        TotalePrice: product.price * product.quantity
      }));

      

      this.orderService.createOrder(orderProducts).subscribe({
        next: (res) => {
          console.log('Order submitted:', res);
          alert('Order submitted successfully!');
          this.cartService.updateCart([]); // Clear the cart after successful order submission
        },
        error: (err:any) => {
          console.log('Order submission error:', err.error);
          alert("There was an error submitting your order. : " +  err.error  +"  Please try again.");
        }
      });
    }
  }
}
