import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ProductIntervalComponent } from './product-interval.component';

describe('ProductIntervalComponent', () => {
  let component: ProductIntervalComponent;
  let fixture: ComponentFixture<ProductIntervalComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ProductIntervalComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ProductIntervalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
