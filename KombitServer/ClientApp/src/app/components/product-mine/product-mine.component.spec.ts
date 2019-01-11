import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductMineComponent } from './product-mine.component';

describe('ProductMineComponent', () => {
  let component: ProductMineComponent;
  let fixture: ComponentFixture<ProductMineComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductMineComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductMineComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
