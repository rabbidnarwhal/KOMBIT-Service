import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../services/auth.service';
import { ProductService } from '../services/product.service';
import { NzMessageService } from 'ng-zorro-antd';

@Injectable({
  providedIn: 'root'
})
export class ProductPosterGuard implements CanActivate {
  constructor(
    private route: Router,
    private authService: AuthService,
    private productService: ProductService,
    private message: NzMessageService
  ) {}

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> | Promise<boolean> | boolean {
    if (this.authService.getUserId() === this.productService.getProductPosterId()) {
      return true;
    } else {
      this.route.navigate([ '' ]);
      this.message.error('Unable to edit others product post.', {
        nzDuration: 5000,
        nzPauseOnHover: true,
        nzAnimate: true
      });
      return false;
    }
  }
}
