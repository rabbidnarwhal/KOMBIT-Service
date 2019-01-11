import { Component, OnInit, Input } from '@angular/core';
import { EventsService } from 'src/app/services/events.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-product-filter',
  templateUrl: './product-filter.component.html',
  styleUrls: [ './product-filter.component.scss' ]
})
export class ProductFilterComponent implements OnInit {
  @Input() products: Array<any>;
  @Input() modal = false;
  @Input() header: any;
  productItem: Array<any>;
  constructor(private eventsService: EventsService, private authService: AuthService) {
    this.productItem = [
      {
        name: 'Company',
        value: 'companyName'
      },
      {
        name: 'Holding',
        value: 'holdingName'
      },
      {
        name: 'Solution',
        value: 'categoryName'
      }
    ];
  }

  ngOnInit() {}

  openDescription(product: any) {
    const content = {
      state: true,
      type: 'description',
      id: product.id,
      modal: this.modal,
      myPost: this.authService.getUserId() === product.posterId
    };
    this.eventsService.setModalState(content);
  }
}
