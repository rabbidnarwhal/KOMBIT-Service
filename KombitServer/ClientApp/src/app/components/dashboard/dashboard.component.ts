import { Component, OnInit } from '@angular/core';
import { ChartjsModule } from '@ctrl/ngx-chartjs';
import { DashboardService } from 'src/app/services/dashboard.service';
import { NzMessageService } from 'ng-zorro-antd';
import { ActiveCustomer } from 'src/app/models/dashboard-response';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: [ './dashboard.component.scss' ]
})
export class DashboardComponent implements OnInit {
  dataCustomer: any;
  dataSupplier: any;
  dataProduct: any;

  start = 0;
  max = 10;
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
        const data = this.slicingData(res);
        const backgroundColorLike = [];
        const backgroundColorComment = [];
        const backgroundColorChat = [];
        const borderColorLike = [];
        const borderColorComment = [];
        const borderColorChat = [];
        const labels = data.name.map((x) => {
          backgroundColorLike.push('rgba(255, 99, 132, 0.2)');
          backgroundColorComment.push('rgba(54, 162, 235, 0.2)');
          backgroundColorChat.push('rgba(255, 206, 86, 0.2)');
          borderColorLike.push('rgba(255,99,132,1)');
          borderColorComment.push('rgba(54, 162, 235, 1)');
          borderColorChat.push('rgba(255, 206, 86, 1)');
          return x.split(' ');
        });
        this.dataCustomer = {
          labels: labels,
          datasets: [
            {
              label: 'Like',
              data: data.totalLike,
              fill: false,
              backgroundColor: backgroundColorLike,
              borderColor: borderColorLike,
              borderWidth: 1
            },
            {
              label: 'Comments',
              data: data.totalComment,
              fill: false,
              backgroundColor: backgroundColorComment,
              borderColor: borderColorComment,
              borderWidth: 1
            },
            {
              label: 'Call',
              data: data.totalChat,
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
        const data = this.slicingData(res);
        const backgroundColorProduct = [];
        const borderColorProduct = [];
        const labels = data.name.map((x: string) => {
          backgroundColorProduct.push('rgba(255, 99, 132, 0.2)');
          borderColorProduct.push('rgba(255, 99, 132, 1)');
          return x.split(' ');
        });
        this.dataSupplier = {
          labels: labels,
          datasets: [
            {
              label: 'Product Uploaded',
              data: data.totalProduct,
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
        const data = this.slicingData(res);
        const backgroundColorLike = [];
        const backgroundColorComment = [];
        const borderColorLike = [];
        const borderColorComment = [];
        const labels = data.productName.map((x) => {
          backgroundColorLike.push('rgba(255, 99, 132, 0.2)');
          backgroundColorComment.push('rgba(54, 162, 235, 0.2)');
          borderColorLike.push('rgba(255,99,132,1)');
          borderColorComment.push('rgba(54, 162, 235, 1)');
          return x.split(' ');
        });
        this.dataProduct = {
          labels: labels,
          datasets: [
            {
              label: 'Like',
              data: data.totalLike,
              fill: false,
              backgroundColor: backgroundColorLike,
              borderColor: borderColorLike,
              borderWidth: 1
            },
            {
              label: 'Comments',
              data: data.totalComment,
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

  slicingData(data: any): any {
    const slicedData = {};
    for (const key in data) {
      if (data.hasOwnProperty(key)) {
        const element = data[key];
        slicedData[key] = element.slice(this.start, this.max);
      }
    }
    return slicedData;
  }

  moreContent(type: string) {
    alert('not yet implemented');
  }
}
