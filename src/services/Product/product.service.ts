import { Injectable } from '@angular/core';
import { ApiService } from '../API/api.service';
import { Product } from 'src/Data/interfaces/product';
import { environment } from 'src/environments/environment';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
  export class ProductService {

    private baseUrl = environment.apiUrl+"/Products";

    constructor(private productApi: ApiService<Product>) {
    }
  
    getAllProducts(additionURL:string="") {
      
      return  this.productApi.get(this.baseUrl+additionURL);
    }
  
    getProductById(id: number) {
      return  this.productApi.getById(this.baseUrl, id);
    }
  
    createProduct(product: Product) {
      return  this.productApi.create(this.baseUrl, product);
    }
  
    updateProduct(id: number, product: Product) {
      return  this.productApi.update(this.baseUrl, id, product);
    }
  
    deleteProduct(id: number) {
      return  this.productApi.delete(this.baseUrl, id);
    }
   
  }