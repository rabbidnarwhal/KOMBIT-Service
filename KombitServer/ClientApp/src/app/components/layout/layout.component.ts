import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, NavigationEnd } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-layout',
  templateUrl: './layout.component.html',
  styleUrls: [ './layout.component.scss' ]
})
export class LayoutComponent implements OnInit {
  isCollapsed = false;
  triggerTemplate = null;
  selectedSubmenu = '';
  breadcrumbsText = 'asdasd';
  userName = 'User';

  constructor(private route: Router, private activatedRoute: ActivatedRoute, private authService: AuthService) {
    this.route.events.subscribe((res) => {
      if (res instanceof NavigationEnd) {
        this.selectedSubmenu = res.url.split('/')[1];
        this.breadcrumbsText = this.activatedRoute.children.length
          ? this.activatedRoute.children[0].snapshot.data.breadcrumb
          : null;
      }
    });
    this.selectedSubmenu = this.route.url.split('/')[1];
  }

  ngOnInit() {
    this.userName = this.authService.getUserName();
  }

  setRoute(event, path) {
    event.stopPropagation();
    this.route.navigate([ path ]);
  }

  logout() {
    this.authService.logout();
    this.userName = 'User';
  }
}
