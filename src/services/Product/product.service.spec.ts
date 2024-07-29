import { TestBed } from '@angular/core/testing';

import { ProductService } from './product.service';
import { ApiService } from '../API/api.service';
import { of } from 'rxjs';
import { Product } from 'src/Data/interfaces/product';

describe('ProductService', () => {
  let service: ProductService;
  let apiService: jasmine.SpyObj<ApiService<Product>>;

  beforeEach(() => {
    const apiServiceSpy = jasmine.createSpyObj('ApiService', [
      'get',
      'getById',
      'create',
      'update',
      'delete'
    ]);

    TestBed.configureTestingModule({
      providers: [
        ProductService,
        { provide: ApiService, useValue: apiServiceSpy }
      ]
    });

    service = TestBed.inject(ProductService);
    apiService = TestBed.inject(ApiService) as jasmine.SpyObj<ApiService<Product>>;
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  describe('getAllProducts', () => {
    it('should call ApiService.get with correct URL', () => {
      // Arrange
      const additionURL = '/featured';
      const products: Product[] = [
        { id: 1, name: 'Product A', description: 'Desc A', price: 100, quantity: 10, isVisible: true }
      ];
      apiService.get.and.returnValue(of(products));

      // Act
      service.getAllProducts(additionURL).subscribe(result => {
        // Assert
        expect(apiService.get).toHaveBeenCalledWith(`${service['baseUrl']}${additionURL}`);
        expect(result).toEqual(products);
      });
    });

    it('should call ApiService.get with default URL when no additionURL is provided', () => {
      // Arrange
      const products: Product[] = [
        { id: 1, name: 'Product A', description: 'Desc A', price: 100, quantity: 10, isVisible: true }
      ];
      apiService.get.and.returnValue(of(products));

      // Act
      service.getAllProducts().subscribe(result => {
        // Assert
        expect(apiService.get).toHaveBeenCalledWith(service['baseUrl']);
        expect(result).toEqual(products);
      });
    });
  });

  describe('getProductById', () => {
    it('should call ApiService.getById with correct URL and ID', () => {
      // Arrange
      const id = 1;
      const product: Product = { id: 1, name: 'Product A', description: 'Desc A', price: 100, quantity: 10, isVisible: true };
      apiService.getById.and.returnValue(of(product));

      // Act
      service.getProductById(id).subscribe(result => {
        // Assert
        expect(apiService.getById).toHaveBeenCalledWith(service['baseUrl'], id);
        expect(result).toEqual(product);
      });
    });
  });

  describe('createProduct', () => {
    it('should call ApiService.create with correct URL and product', () => {
      // Arrange
      const product: Product = { id: 1, name: 'Product A', description: 'Desc A', price: 100, quantity: 10, isVisible: true };
      apiService.create.and.returnValue(of(product));

      // Act
      service.createProduct(product).subscribe(result => {
        // Assert
        expect(apiService.create).toHaveBeenCalledWith(service['baseUrl'], product);
        expect(result).toEqual(product);
      });
    });
  });

  describe('updateProduct', () => {
    it('should call ApiService.update with correct URL, ID, and product', () => {
      // Arrange
      const id = 1;
      const product: Product = { id: 1, name: 'Product A', description: 'Desc A', price: 100, quantity: 10, isVisible: true };
      apiService.update.and.returnValue(of(product));

      // Act
      service.updateProduct(id, product).subscribe(result => {
        // Assert
        expect(apiService.update).toHaveBeenCalledWith(service['baseUrl'], id, product);
        expect(result).toEqual(product);
      });
    });
  });

  
});