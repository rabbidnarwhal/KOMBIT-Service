import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { Subscription } from 'rxjs';
import { DashboardService } from 'src/app/services/dashboard.service';
import { ActiveCustomer, ActiveSupplier, MostPopularProduct } from 'src/app/models/dashboard-response';

@Component({
  selector: 'app-dashboard-table-view',
  templateUrl: './dashboard-table-view.component.html',
  styleUrls: [ './dashboard-table-view.component.scss' ]
})
export class DashboardTableViewComponent implements OnInit, OnDestroy {
  subscription: Subscription;
  title: string;
  headers: Array<any>;
  dataSet: Array<any> = [];
  loading = true;

  displayData: Array<any> = [];
  sortValue = null;
  sortKey = null;
  pageIndex = 1;
  pageSize = 10;
  constructor(private activatedRoute: ActivatedRoute, private dashboardService: DashboardService) {}

  ngOnInit() {
    this.subscribeDashboardParams();
  }

  ngOnDestroy() {
    this.subscription.unsubscribe();
  }

  subscribeDashboardParams() {
    this.subscription = this.activatedRoute.queryParams.subscribe(async (res) => {
      if (res.hasOwnProperty('activeCustomer') && res.activeCustomer) {
        this.title = 'Active Customer';
        this.headers = [
          { label: 'Name', value: 'name' },
          { label: 'Views', value: 'views' },
          { label: 'Interactions', value: 'interaction' },
          { label: 'Likes', value: 'like' },
          { label: 'Comments', value: 'comment' },
          { label: 'Phones', value: 'chat' }
        ];
        const data: ActiveCustomer = await this.dashboardService.getActiveCustomer();
        this.dataSet = data.name.map((x, index) => {
          const object = {
            chat: data.totalChat[index],
            comment: data.totalComment[index],
            interaction: data.totalInteraction[index],
            like: data.totalLike[index],
            views: data.totalView[index],
            name: data.name[index]
          };
          return object;
        });
        this.loading = false;
      } else if (res.hasOwnProperty('activeSupplier') && res.activeSupplier) {
        this.title = 'Active Supplier';
        this.headers = [ { label: 'Name', value: 'name' }, { label: 'Total Product', value: 'totalProduct' } ];
        const data: ActiveSupplier = await this.dashboardService.getActiveSupplier();
        this.dataSet = data.name.map((x, index) => {
          const object = {
            totalProduct: data.totalProduct[index],
            name: data.name[index]
          };
          return object;
        });
        this.loading = false;
      } else if (res.hasOwnProperty('mostPopularProduct') && res.mostPopularProduct) {
        this.title = 'Most Popular Product';
        this.headers = [
          { label: 'Product Name', value: 'name' },
          { label: 'Views', value: 'views' },
          { label: 'Interactions', value: 'interaction' },
          { label: 'Likes', value: 'like' },
          { label: 'Comments', value: 'comment' },
          { label: 'Phones', value: 'chat' }
        ];
        const data: MostPopularProduct = await this.dashboardService.getMostPopularProduct();
        this.dataSet = data.productName.map((x, index) => {
          const object = {
            name: data.productName[index],
            chat: data.totalChat[index],
            comment: data.totalComment[index],
            interaction: data.totalInteraction[index],
            like: data.totalLike[index],
            views: data.totalView[index]
          };
          return object;
        });
        this.loading = false;
      }
    });
  }

  sort(sort: { key: string; value: string }): void {
    this.sortKey = sort.key;
    this.sortValue = sort.value;
    // this.searchData();
  }

  currentPageDataChange($event: Array<any>): void {
    this.displayData = $event;
  }
}
