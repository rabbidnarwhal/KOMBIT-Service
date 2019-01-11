import { Component, OnInit, Input } from '@angular/core';
import { EventsService } from 'src/app/services/events.service';

@Component({
  selector: 'app-product-mine',
  templateUrl: './product-mine.component.html',
  styleUrls: [ './product-mine.component.scss' ]
})
export class ProductMineComponent implements OnInit {
  @Input() products: Array<any>;
  @Input() header: any;

  constructor(private eventsService: EventsService) {}

  ngOnInit() {}

  openDescription(productId: number) {
    const content = {
      state: true,
      type: 'description',
      id: productId,
      modal: true,
      myPost: true
    };
    this.eventsService.setModalState(content);
  }
}
