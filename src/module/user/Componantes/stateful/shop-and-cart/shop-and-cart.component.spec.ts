import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShopAndCartComponent } from './shop-and-cart.component';

describe('ShopAndCartComponent', () => {
  let component: ShopAndCartComponent;
  let fixture: ComponentFixture<ShopAndCartComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShopAndCartComponent ]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShopAndCartComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
