import { Component, OnInit, NgZone, OnDestroy } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { Router, NavigationEnd } from '@angular/router';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-layout-public',
  templateUrl: './layout-public.component.html',
  styleUrls: [ './layout-public.component.scss' ]
})
export class LayoutPublicComponent implements OnInit {
  isLogin = false;
  role = '';
  loginSubscription: Subscription;
  constructor(private authService: AuthService) {}

  async ngOnInit() {
    await this.authService.isAuthenticated();
    this.checkLogin();
  }

  checkLogin() {
    this.loginSubscription = this.authService.isLoggin.subscribe((isLogin) => {
      this.isLogin = isLogin;
      this.role = this.authService.getRole();
    });
  }
}
