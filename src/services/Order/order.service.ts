import { Injectable } from '@angular/core';
import { Order } from 'src/Data/interfaces/order';
import { environment } from 'src/environments/environment';
import { ApiService } from '../API/api.service';
import { OrderProduct } from 'src/Data/interfaces/order-product';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  private baseUrl = environment.apiUrl+"/Orders";
  
    constructor(private OrderApi: ApiService<OrderProduct[]>) {
    }
  
  
    createOrder(orderProduct: OrderProduct[]) {
      return  this.OrderApi.create(this.baseUrl, orderProduct);
    }
  
  
  
}
