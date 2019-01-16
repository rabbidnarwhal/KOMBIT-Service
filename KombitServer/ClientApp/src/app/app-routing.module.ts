import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { DashboardComponent } from './components/dashboard/dashboard.component';
import { LayoutComponent } from './components/layout/layout.component';
import { LoginComponent } from './components/login/login.component';
import { LayoutPublicComponent } from './components/layout-public/layout-public.component';
import { ProductListComponent } from './components/product-list/product-list.component';
import { ProductPostComponent } from './components/product-post/product-post.component';
import { RoleGuard } from './guards/role.guard';
import { ProductPosterGuard } from './guards/product-poster.guard';
import { ProductListTableComponent } from './components/product-list-table/product-list-table.component';
import { ProductDetailComponent } from './components/product-detail/product-detail.component';
import { DashboardTableViewComponent } from './components/dashboard-table-view/dashboard-table-view.component';

const routes: Routes = [
  {
    path: '',
    component: LayoutPublicComponent,
    children: [
      {
        path: '',
        component: ProductListComponent
      }
    ]
  },
  {
    path: 'dashboard',
    canActivate: [ RoleGuard ],
    component: LayoutComponent,
    canActivateChild: [ AuthGuard ],
    data: {
      expectedRole: 'Administrator'
    },
    children: [
      {
        path: '',
        component: DashboardComponent,
        data: {
          breadcrumb: 'Dashboard'
        }
      },

      {
        path: 'product',
        component: ProductListTableComponent,
        data: {
          breadcrumb: 'Product'
        }
      },
      {
        path: ':dashboardView',
        component: DashboardTableViewComponent,
        data: {
          breadcrumb: 'Dashboard'
        }
      }
    ]
  },
  {
    path: 'product',
    component: LayoutPublicComponent,

    children: [
      {
        path: '',
        redirectTo: '/',
        pathMatch: 'full'
      },
      {
        path: 'new',
        canActivate: [ RoleGuard ],
        canActivateChild: [ AuthGuard ],
        data: {
          expectedRole: 'Supplier'
        },
        children: [
          {
            path: '',
            component: ProductPostComponent,
            data: {
              breadcrumb: 'Post Product'
            }
          }
        ]
      },
      {
        path: 'edit',
        canActivate: [ RoleGuard ],
        canActivateChild: [ AuthGuard ],
        data: {
          expectedRole: 'Supplier'
        },
        children: [
          {
            path: ':productId',
            canActivate: [ ProductPosterGuard ],
            component: ProductPostComponent,
            data: {
              breadcrumb: 'Edit Product'
            }
          }
        ]
      },
      {
        path: ':productId',
        component: ProductListComponent
      }
    ]
  },
  {
    path: 'login',
    component: LoginComponent
  },
  {
    path: '**',
    redirectTo: '/'
  },
  {
    path: 'not-found',
    component: LayoutPublicComponent
  }
];

@NgModule({
  imports: [ RouterModule.forRoot(routes, { useHash: false }) ],
  exports: [ RouterModule ]
})
export class AppRoutingModule {}
