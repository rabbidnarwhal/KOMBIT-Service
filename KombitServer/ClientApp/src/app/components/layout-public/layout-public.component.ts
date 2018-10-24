import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-layout-public',
  templateUrl: './layout-public.component.html',
  styleUrls: [ './layout-public.component.scss' ]
})
export class LayoutPublicComponent implements OnInit {
  isLogin = false;
  isSupplier = true;
  userName = 'User';
  constructor(private authService: AuthService, private route: Router) {}

  ngOnInit() {
    this.authService.isAuthenticated().then((res) => {
      this.isLogin = res;
      this.isSupplier = this.authService.getRole() === 'Supplier' ? true : false;
      this.userName = this.authService.getUserName();
    });
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
