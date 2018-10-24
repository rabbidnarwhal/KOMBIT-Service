import { TestBed, async, inject } from '@angular/core/testing';

import { ProductPosterGuard } from './product-poster.guard';

describe('ProductPosterGuard', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [ProductPosterGuard]
    });
  });

  it('should ...', inject([ProductPosterGuard], (guard: ProductPosterGuard) => {
    expect(guard).toBeTruthy();
  }));
});
