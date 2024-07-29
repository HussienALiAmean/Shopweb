import { Component, OnInit } from '@angular/core';
import { Product } from 'src/Data/interfaces/product';
import { CartService } from 'src/services/Cart/cart.service';
import { ProductService } from 'src/services/Product/product.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit {

  products: Product[] = [];
  filteredProducts: Product[] = [];
  searchQuery: string = '';

  constructor(private productService: ProductService, private cartService: CartService) { }

  ngOnInit(): void {
    this.productService.getAllProducts("/visible").subscribe(products => {
      this.products = products;
      this.filteredProducts = products;
    });
  }

  addToCart(product: Product) {
    this.cartService.addToCart(product);
  }

  filterProducts() {
    this.filteredProducts = this.products.filter(product => 
      product.name.toLowerCase().includes(this.searchQuery.toLowerCase())
    );
  }
}
