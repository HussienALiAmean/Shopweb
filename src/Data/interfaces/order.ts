import { OrderProduct } from "./order-product";

export interface Order {
    id?: number;
    userId?: string;
    orderDate?: Date;
    orderProduct: OrderProduct[];
}