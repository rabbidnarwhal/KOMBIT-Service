import { Component, OnInit, OnDestroy, Renderer2, ElementRef, ViewChild } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { AuthService } from 'src/app/services/auth.service';
import { Router, ActivatedRoute } from '@angular/router';
import { NzMessageService } from 'ng-zorro-antd';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { EventsService } from 'src/app/services/events.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-product-list',
  templateUrl: './product-list.component.html',
  styleUrls: [ './product-list.component.scss' ],
  animations: [
    trigger('simpleFadeAnimation', [
      state('in', style({ opacity: 1 })),

      transition(':enter', [ style({ opacity: 0 }), animate(600) ]),

      transition(':leave', animate(300, style({ opacity: 0 })))
    ])
  ]
})
export class ProductListComponent implements OnInit, OnDestroy {
  @ViewChild('productList') elemRef: ElementRef;
  originProduct = [];
  favoriteProduct = [];
  products = [];
  filterProducts = [];
  skeletons = [ 0, 0, 0, 0, 0 ];
  userId = 0;
  updatedId = 0;
  scheduleId = 0;
  productQueryParamSubscription: Subscription;
  productUrlSubscription: Subscription;
  productSearchSubscription: Subscription;
  isLoading = true;

  isModal = false;
  editable = false;
  scrollPosition = 0;
  selectedPost = 0;

  modalHeader: any = { icon: '', text: '', type: '' };
  modalSubscription: Subscription;
  modalType = '';

  constructor(
    private productService: ProductService,
    private authService: AuthService,
    private activatedRoute: ActivatedRoute,
    private message: NzMessageService,
    private renderer: Renderer2,
    private eventsService: EventsService
  ) {
    this.products = this.skeletons;
  }

  subscribeProductEvent() {
    this.productQueryParamSubscription = this.activatedRoute.queryParams.subscribe((res) => {
      if (res.hasOwnProperty('myPost') && res.myPost) {
        alert('myPost');
      } else if (res.hasOwnProperty('new') && res.new) {
        alert('newPost');
      } else if (res.hasOwnProperty('post') && res.post > 0) {
        this.openDescription(res.post);
      } else if (res.hasOwnProperty('post') && res.post && res.hasOwnProperty('edit') && res.edit) {
        alert('edit product');
      }
    });

    this.productUrlSubscription = this.activatedRoute.url.subscribe((res) => {
      if (res.length) {
        if (res[0].hasOwnProperty('path')) {
          this.openDescription(+res[0].path);
        }
      }
    });
  }

  subscribeSearchProductEvent() {
    this.productSearchSubscription = this.eventsService.getProductSearchText().subscribe((res) => {
      if (res) {
        this.products = this.originProduct.filter((x) => x.productName.toLowerCase().indexOf(res.toLowerCase()) > -1);
      } else {
        this.products = this.originProduct;
      }
    });
  }

  subscribeModalEvent() {
    this.modalSubscription = this.eventsService.getModalState().subscribe(async (subs) => {
      this.modalType = subs.type;
      this.modalHeader = subs.header;
      this.filterProducts = [];
      if (this.modalType === 'description') {
        this.selectedPost = subs.id;
        this.editable = subs.myPost || false;
      }

      if (this.modalType === 'updateProduct') {
        this.updatedId = subs.updatedId || 0;
      }

      if (this.modalType === 'filter') {
        this.filterProducts = this.originProduct.filter((x) => x.categoryId === subs.id);
      }

      if (this.modalType === 'favorite') {
        this.modalType = 'filter';
        this.filterProducts = this.originProduct.filter((x) => x.isLike);
      }

      if (this.modalType === 'myPost') {
        this.filterProducts = this.originProduct.filter((x) => x.posterId === this.userId);
      }

      if (this.modalType === 'meetingDetail') {
        this.scheduleId = subs.scheduleId || 0;
      }

      if (subs.state && !this.isModal) {
        this.isModal = true;
        this.scrollPosition = window.pageYOffset;
        this.elemRef.nativeElement.style.position = 'fixed';
        this.elemRef.nativeElement.style.top = -this.scrollPosition + 'px';
        this.renderer.addClass(document.body, 'modal-open');
      } else if (!subs.state && this.isModal) {
        this.isModal = false;
        setTimeout(() => {
          this.renderer.removeClass(document.body, 'modal-open');
          this.elemRef.nativeElement.style.position = 'inherit';
          this.elemRef.nativeElement.style.top = 0 + 'px';
          window.scrollTo({ top: this.scrollPosition });
        }, 300);
        window.history.replaceState({}, '', '/');
        if (this.modalType === 'deleteProduct') {
          this.isLoading = true;
          this.products = this.skeletons;
          try {
            this.originProduct = await this.productService.getListProduct();
            this.products = this.originProduct;
          } catch (error) {
            this.message.error(error);
          }
          this.isLoading = false;
        }
      }
    });
  }

  async ngOnInit() {
    this.subscribeModalEvent();
    this.subscribeProductEvent();
    this.subscribeSearchProductEvent();
    try {
      this.originProduct = await this.productService.getListProduct();
      this.productService.setProductPosterId();
      this.products = this.originProduct;
      this.isLoading = false;
      this.checkLogin();
    } catch (error) {
      this.message.error(error, { nzDuration: 5000, nzPauseOnHover: true, nzAnimate: true });
    }
  }

  ngOnDestroy() {
    this.modalSubscription.unsubscribe();
    this.productQueryParamSubscription.unsubscribe();
    this.productSearchSubscription.unsubscribe();
    this.productUrlSubscription.unsubscribe();
  }

  openDescription(productId: number) {
    const content = {
      state: true,
      type: 'description',
      id: productId
    };
    this.eventsService.setModalState(content);
  }

  closeModal() {
    const content = {
      state: false,
      type: ''
    };
    this.eventsService.setModalState(content);
  }

  checkLogin() {
    this.authService.isLoggin.subscribe((isLogin) => {
      if (isLogin) {
        this.userId = this.authService.getUserId();
        this.productService.getListLikedProduct(this.userId).then((res) => {
          this.originProduct = res;
        });
      } else {
        this.userId = 0;
      }
    });
  }

  openUpdateProduct(id = 0) {
    const content = {
      state: true,
      type: 'updateProduct',
      id: id
    };
    this.eventsService.setModalState(content);
  }
}
