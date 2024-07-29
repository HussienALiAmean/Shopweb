import { TestBed } from '@angular/core/testing';

import { CartService } from './cart.service';
import { Product } from 'src/Data/interfaces/product';

describe('CartService', () => {
  let service: CartService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [CartService]
    });
    service = TestBed.inject(CartService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('addToCart', () => {
    it('should add a new product to the cart', () => {
      // Arrange
      const product: Product = { id: 1, name: 'Test Product', description: 'Test Description', price: 100, quantity: 1, isVisible: true };

      // Act
      service.addToCart(product);

      // Assert
      service.cart$.subscribe(cart => {
        expect(cart.length).toBe(1);
        expect(cart[0]).toEqual({ ...product, quantity: 1 });
      });
    });

    it('should increase quantity of existing product in the cart', () => {
      // Arrange
      const product: Product = { id: 1, name: 'Test Product', description: 'Test Description', price: 100, quantity: 1, isVisible: true };
      service.addToCart(product); // Add product once

      // Act
      service.addToCart(product); // Add product again

      // Assert
      service.cart$.subscribe(cart => {
        expect(cart.length).toBe(1);
        expect(cart[0].quantity).toBe(2);
      });
    });
  });

  describe('getCartItems', () => {
    it('should return the current items in the cart', () => {
      // Arrange
      const product: Product = { id: 1, name: 'Test Product', description: 'Test Description', price: 100, quantity: 1, isVisible: true };
      service.addToCart(product);

      // Act
      const items = service.getCartItems();

      // Assert
      expect(items.length).toBe(1);
      expect(items[0]).toEqual({ ...product, quantity: 1 });
    });
  });

  describe('updateCart', () => {
    it('should update the cart with new items', () => {
      // Arrange
      const initialProduct: Product = { id: 1, name: 'Initial Product', description: 'Initial Description', price: 100, quantity: 1, isVisible: true };
      service.addToCart(initialProduct);

      const updatedProduct: Product = { id: 2, name: 'Updated Product', description: 'Updated Description', price: 200, quantity: 3, isVisible: true };
      service.updateCart([updatedProduct]);

      // Act & Assert
      service.cart$.subscribe(cart => {
        expect(cart.length).toBe(1);
        expect(cart[0]).toEqual(updatedProduct);
      });
    });
  });
});