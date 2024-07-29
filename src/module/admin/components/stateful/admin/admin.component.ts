import { Component, OnInit } from '@angular/core';
import { Observable, Subscription } from 'rxjs';
import { Product } from 'src/Data/interfaces/product';
import { ProductService } from 'src/services/Product/product.service';

@Component({
  selector: 'app-admin',
  templateUrl: './admin.component.html',
  styleUrls: ['./admin.component.css']
})
export class AdminComponent implements OnInit {

  getAllProductsSubscription!:Subscription;
  updateProductSubscription!:Subscription;
  deleteProductSubscription!: Subscription;
  Products!:Product[];
  filteredProducts!: Product[];
  filterText: string = ''; // To store filter text
  newProduct: Product = { id: 0, name: '', description:"", price: 0, quantity: 0, isVisible: false };
  isSortAsc: boolean = true; // To track sorting order

  constructor(private productService: ProductService) {
    
  }

  ngOnInit(): void {
    this.getAllProducts();
  }
  
  getAllProducts(): void {
    this.getAllProductsSubscription = this.productService.getAllProducts().subscribe({
      next: (res) => {
        this.Products = res;
        this.filteredProducts = res; // Initialize filtered products
        this.newProduct = { id: 0, name: '', description: '', price: 0, quantity: 0, isVisible: false };
      },
      error: () => { alert("error"); }
    });
  }

  sortByName(): void {
    this.isSortAsc = !this.isSortAsc; // Toggle sorting order
    this.filteredProducts.sort((a, b) => {
      if (this.isSortAsc) {
        return a.name.localeCompare(b.name);
      } else {
        return b.name.localeCompare(a.name);
      }
    });
  }
  filterProducts(): void {
    this.filteredProducts = this.Products.filter(product => 
      product.name.toLowerCase().includes(this.filterText.toLowerCase())
    );
  }
  updateProduct(product: Product): void {
    this.updateProductSubscription = this.productService.updateProduct(product.id,product).subscribe({
     next:()=>{this.getAllProducts();},
     error:(err)=>{ console.log(err);}
     });
  }

  addProduct(): void {
    if (this.newProduct.name && this.newProduct.price && this.newProduct.quantity) {
      this.productService.createProduct(this.newProduct).subscribe({
        next:(res)=>
          { console.log(res);
            this.getAllProducts();},
        error:(err)=>{ console.log(err);}
      });
    }
  }

  deleteProduct(productId: number): void {
    this.deleteProductSubscription = this.productService.deleteProduct(productId).subscribe({
      next: () => { this.getAllProducts(); },
      error: (err) => { console.log(err); }
    });
  }

  ngOnDestroy(): void {
    this.getAllProductsSubscription.unsubscribe();
    this.updateProductSubscription.unsubscribe();
    this.deleteProductSubscription.unsubscribe();
    
  }
}

