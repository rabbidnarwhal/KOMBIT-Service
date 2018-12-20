import { Component, OnInit, NgZone, OnDestroy } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router, NavigationEnd } from '@angular/router';

@Component({
  selector: 'app-layout-public',
  templateUrl: './layout-public.component.html',
  styleUrls: [ './layout-public.component.scss' ]
})
export class LayoutPublicComponent implements OnInit, OnDestroy {
  isLogin = false;
  isSupplier = true;
  isSider = true;
  userName = 'User';
  subscription: any;
  constructor(private authService: AuthService, private route: Router, private zone: NgZone) {
    this.subscription = this.route.events.subscribe((subs) => {
      if (subs instanceof NavigationEnd) {
        console.log('subs', subs);
        if (
          subs.urlAfterRedirects.includes('new') ||
          subs.urlAfterRedirects.includes('edit') ||
          subs.urlAfterRedirects.includes('product')
        ) {
          this.isSider = false;
        } else {
          this.isSider = true;
        }
      }
    });
  }

  ngOnInit() {
    this.authService.isAuthenticated().then((res) => {
      this.isLogin = res;
      this.isSupplier = this.authService.getRole() === 'Supplier' ? true : false;
      this.userName = this.authService.getUserName();
    });
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  toMemberArea() {
    if (this.isSupplier) {
      this.route.navigate([ '/product/new' ]);
    } else {
      this.route.navigate([ '/dashboard' ]);
    }
  }

  logout() {
    this.authService.logout();
    this.isLogin = false;
    this.userName = 'User';
    this.isSupplier = true;
  }
}
