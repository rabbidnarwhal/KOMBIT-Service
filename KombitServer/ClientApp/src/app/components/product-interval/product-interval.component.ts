import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { NzModalRef } from 'ng-zorro-antd';

@Component({
  selector: 'app-product-interval',
  templateUrl: './product-interval.component.html',
  styleUrls: [ './product-interval.component.scss' ]
})
export class ProductIntervalComponent implements OnInit {
  intervalData = [];
  intervalForm: FormGroup;
  // intervalProduct: any;

  constructor(private modal: NzModalRef, private productService: ProductService, private fb: FormBuilder) {
    this.intervalData = [ { type: 'month', label: 'Month(s)' }, { type: 'day', label: 'Day(s)' } ];
    // this.intervalProduct = {value: 0, type: ''};
  }

  ngOnInit() {
    this.intervalForm = this.fb.group({
      interval: [ null, [ Validators.required, Validators.min(1) ] ],
      intervalType: [ 'day' ]
    });

    this.productService.getIntervalProduct().then((res) => {
      this.intervalForm.get('interval').setValue(res.value);
      this.intervalForm.get('interval').updateValueAndValidity();
      this.intervalForm.get('intervalType').setValue(res.type);
      this.intervalForm.get('intervalType').updateValueAndValidity();
    });
  }

  closeModal() {
    this.modal.destroy();
  }

  changeInterval(): void {
    for (const key in this.intervalForm.controls) {
      if (this.intervalForm.controls.hasOwnProperty(key)) {
        this.intervalForm.controls[key].markAsDirty();
        this.intervalForm.controls[key].updateValueAndValidity();
      }
    }
    if (this.intervalForm.valid) {
      const interval = this.intervalForm.get('interval').value;
      const type = this.intervalForm.get('intervalType').value;
      this.modal.destroy({ interval: interval, type: type });
    }
  }
}
