import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { NzMessageService, NzModalService } from 'ng-zorro-antd';
import { ProductIntervalComponent } from '../product-interval/product-interval.component';

@Component({
  selector: 'app-product-list-table',
  templateUrl: './product-list-table.component.html',
  styleUrls: [ './product-list-table.component.scss' ]
})
export class ProductListTableComponent implements OnInit {
  allChecked = false;
  disabledButton = true;
  checkedNumber = 0;
  displayData: Array<any> = [];
  operating = false;
  dataSet = [];
  indeterminate = false;

  sortValue = null;
  sortKey = null;
  pageIndex = 1;
  pageSize = 10;
  product = [];
  loading = true;
  searchString = '';
  productInterval: any;
  constructor(
    private productServices: ProductService,
    private msg: NzMessageService,
    private modalService: NzModalService
  ) {}

  ngOnInit() {
    this.searchData();
  }

  getListProduct(): Promise<Array<any>> {
    return this.productServices.getListAllProduct();
  }

  getProductInterval(): Promise<any> {
    return this.productServices.getIntervalProduct();
  }

  async searchData() {
    this.loading = true;
    try {
      this.product = await this.getListProduct();
      this.productInterval = await this.getProductInterval();
      this.searchString = '';
      this.updateFilter();
    } catch (error) {
      this.msg.error(error);
    }
    this.dataSet = this.product;
    this.loading = false;
  }

  sort(sort: { key: string; value: string }): void {
    this.sortKey = sort.key;
    this.sortValue = sort.value;
    // this.searchData();
  }

  updateFilter(): void {
    this.dataSet = this.product.filter((x) => {
      for (const key in x) {
        if (x.hasOwnProperty(key)) {
          const element: string = x[key] + '';
          if (element.toLowerCase().includes(this.searchString.trim().toLowerCase())) {
            return x;
          }
        }
      }
    });
  }

  promoteProduct(value: any = null, promote = true) {
    const promotePromise = [];
    let message = '';
    if (value) {
      if (value.isPromoted) {
        promotePromise.push(this.productServices.demoteProduct(value.id));
        message = 'demoted';
      } else {
        promotePromise.push(this.productServices.promoteProduct(value.id));
        message = 'promoted';
      }
    } else {
      const marked = this.dataSet.filter((x) => x.checked);
      marked.map((x) => {
        if (promote) {
          promotePromise.push(this.productServices.promoteProduct(x.id));
          message = 'promoted';
        } else {
          promotePromise.push(this.productServices.demoteProduct(x.id));
          message = 'demoted';
        }
      });
    }
    Promise.all(promotePromise)
      .then((res) => {
        if (promotePromise.length === 1) {
          this.msg.success(value.productName + ' is ' + message);
        } else {
          this.msg.success(promotePromise.length + ' products are ' + message);
        }
        this.searchData();
      })
      .catch((err) => this.msg.error(err));
  }

  activeProduct(value: any = null, active = true) {
    const activePromise = [];
    let message = '';
    if (value) {
      if (value.isActive) {
        activePromise.push(this.productServices.deactiveProduct(value.id));
        message = 'deactivated';
      } else {
        activePromise.push(this.productServices.activeProduct(value.id));
        message = 'activated';
      }
    } else {
      const marked = this.dataSet.filter((x) => x.checked);
      marked.map((x) => {
        if (active) {
          activePromise.push(this.productServices.activeProduct(x.id));
          message = 'activated';
        } else {
          activePromise.push(this.productServices.deactiveProduct(x.id));
          message = 'deactivated';
        }
      });
    }
    Promise.all(activePromise)
      .then((res) => {
        if (activePromise.length === 1) {
          this.msg.success(value.productName + ' is ' + message);
        } else {
          this.msg.success(activePromise.length + ' products are ' + message);
        }
        this.searchData();
      })
      .catch((err) => this.msg.error(err));
  }

  refreshStatus(): void {
    const allChecked = this.displayData.filter((value) => !value.disabled).every((value) => value.checked === true);
    const allUnChecked = this.displayData.filter((value) => !value.disabled).every((value) => !value.checked);
    this.allChecked = allChecked;
    this.indeterminate = !allChecked && !allUnChecked;
    this.disabledButton = !this.dataSet.some((value) => value.checked);
    this.checkedNumber = this.dataSet.filter((value) => value.checked).length;
  }

  checkAll(value: boolean): void {
    this.displayData.map((data) => {
      if (!data.disabled) {
        data.checked = value;
      }
    });
    this.refreshStatus();
  }

  currentPageDataChange($event: Array<any>): void {
    this.displayData = $event;
    this.refreshStatus();
  }

  changeIntervalProduct() {
    const modal = this.modalService.create({
      nzTitle: 'Product Interval',
      nzContent: ProductIntervalComponent,
      nzWidth: 600,
      nzFooter: [
        {
          label: 'Close',
          onClick: (componentInstance) => {
            componentInstance.closeModal();
          }
        },
        {
          label: 'Save',
          onClick: (componentInstance) => {
            componentInstance.changeInterval();
          }
        }
      ]
    });

    modal.afterClose.subscribe(async (result) => {
      if (result && result.hasOwnProperty('interval') && result.hasOwnProperty('type')) {
        this.loading = true;
        try {
          await this.productServices.setProductInterval(result.interval, result.type);
          this.loading = false;
          this.msg.success('Product update interval changed');
        } catch (error) {
          this.loading = false;
          this.msg.error(error);
        }
      }
    });
  }
}
