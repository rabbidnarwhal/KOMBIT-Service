import { Component, OnInit } from '@angular/core';
import { ChartjsModule } from '@ctrl/ngx-chartjs';
import { DashboardService } from 'src/app/services/dashboard.service';
import { NzMessageService } from 'ng-zorro-antd';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: [ './dashboard.component.scss' ]
})
export class DashboardComponent implements OnInit {
  dataCustomer: any;
  dataSupplier: any;
  dataProduct: any;
  constructor(private dashboardService: DashboardService, private message: NzMessageService) {}

  ngOnInit() {
    this.generateActiveCostumerData();
    this.generateActiveSupplierData();
    this.generateMostPopularProductData();
  }

  generateActiveCostumerData() {
    this.dashboardService
      .getActiveCustomer()
      .then((res) => {
        const backgroundColorLike = [];
        const backgroundColorComment = [];
        const backgroundColorChat = [];
        const borderColorLike = [];
        const borderColorComment = [];
        const borderColorChat = [];
        res.name.map((x) => {
          backgroundColorLike.push('rgba(255, 99, 132, 0.2)');
          backgroundColorComment.push('rgba(54, 162, 235, 0.2)');
          backgroundColorChat.push('rgba(255, 206, 86, 0.2)');
          borderColorLike.push('rgba(255,99,132,1)');
          borderColorComment.push('rgba(54, 162, 235, 1)');
          borderColorChat.push('rgba(255, 206, 86, 1)');
        });
        this.dataCustomer = {
          labels: res.name,
          datasets: [
            {
              label: 'Like',
              data: res.totalLike,
              fill: false,
              backgroundColor: backgroundColorLike,
              borderColor: borderColorLike,
              borderWidth: 1
            },
            {
              label: 'Comments',
              data: res.totalComment,
              fill: false,
              backgroundColor: backgroundColorComment,
              borderColor: borderColorComment,
              borderWidth: 1
            },
            {
              label: 'Call',
              data: res.totalChat,
              fill: false,
              backgroundColor: backgroundColorChat,
              borderColor: borderColorChat,
              borderWidth: 1
            }
          ]
        };
      })
      .catch((err) => {
        this.message.error(err, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
      });
  }
  generateActiveSupplierData() {
    this.dashboardService
      .getActiveSupplier()
      .then((res) => {
        const backgroundColorProduct = [];
        const borderColorProduct = [];
        res.name.map((x) => {
          backgroundColorProduct.push('rgba(255, 99, 132, 0.2)');
          borderColorProduct.push('rgba(255, 99, 132, 1)');
        });
        this.dataSupplier = {
          labels: res.name,
          datasets: [
            {
              label: 'Product Uploaded',
              data: res.totalProduct,
              fill: false,
              backgroundColor: backgroundColorProduct,
              borderColor: borderColorProduct,
              borderWidth: 1
            }
          ]
        };
      })
      .catch((err) => {
        this.message.error(err, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
      });
  }
  generateMostPopularProductData() {
    this.dashboardService
      .getMostPopularProduct()
      .then((res) => {
        const backgroundColorLike = [];
        const backgroundColorComment = [];
        const borderColorLike = [];
        const borderColorComment = [];
        res.productName.map((x) => {
          backgroundColorLike.push('rgba(255, 99, 132, 0.2)');
          backgroundColorComment.push('rgba(54, 162, 235, 0.2)');
          borderColorLike.push('rgba(255,99,132,1)');
          borderColorComment.push('rgba(54, 162, 235, 1)');
        });
        this.dataProduct = {
          labels: res.productName,
          datasets: [
            {
              label: 'Like',
              data: res.totalLike,
              fill: false,
              backgroundColor: backgroundColorLike,
              borderColor: borderColorLike,
              borderWidth: 1
            },
            {
              label: 'Comments',
              data: res.totalComment,
              fill: false,
              backgroundColor: backgroundColorComment,
              borderColor: borderColorComment,
              borderWidth: 1
            }
          ]
        };
      })
      .catch((err) => {
        this.message.error(err, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
      });
  }
}
