import { Component, OnInit, OnDestroy } from '@angular/core';
import { EventsService } from 'src/app/services/events.service';
import { NzMessageService } from 'ng-zorro-antd';
import { ProductService } from 'src/app/services/product.service';
import { AuthService } from 'src/app/services/auth.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-main-menu',
  templateUrl: './main-menu.component.html',
  styleUrls: [ './main-menu.component.scss' ]
})
export class MainMenuComponent implements OnInit, OnDestroy {
  isLoading = true;
  solutions = [];
  filterSolutions = [];
  searchSolution = '';
  isLogin = false;
  role = '';
  loginSubscriber: Subscription;
  constructor(
    private eventsService: EventsService,
    private msgService: NzMessageService,
    private productService: ProductService,
    private authService: AuthService
  ) {}

  async ngOnInit() {
    this.checkLogin();
    try {
      this.solutions = await this.productService.getListSolution();
      // while (this.solutions.length < 12) {
      //   const solution = {
      //     image:
      //       'data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAQAAAC1HAwCAAAAC0lEQVR42mNkYAAAAAYAAjCB0C8AAAAASUVORK5CYII=',
      //     category: '',
      //     id: 0
      //   };
      //   this.solutions = [ ...this.solutions, solution ];
      // }
      this.filterSolution('');
      this.isLoading = false;
    } catch (error) {
      this.msgService.error(error);
    }
  }

  ngOnDestroy() {
    this.loginSubscriber.unsubscribe();
  }

  checkLogin() {
    this.loginSubscriber = this.authService.isLoggin.subscribe((isLogin) => {
      this.isLogin = isLogin;
      this.role = this.authService.getRole();
    });
  }

  openModal(solution: any) {
    if (solution.id) {
      const content = {
        state: true,
        type: 'filter',
        id: solution.id,
        header: {
          icon: solution.image,
          text: solution.category,
          type: 'solutions'
        }
      };
      this.eventsService.setModalState(content);
    }
  }

  openNewPostModal() {
    const content = {
      state: true,
      type: 'updateProduct',
      header: {
        icon: 'assets/images/new-post.png',
        text: 'New Post'
      }
    };
    this.eventsService.setModalState(content);
  }

  filterSolution(text: string) {
    this.filterSolutions = this.solutions.filter(
      (item) => item.category.toLowerCase().indexOf(text.toLowerCase()) > -1
    );
    this.eventsService.setProductSearchText(text);
  }
}
