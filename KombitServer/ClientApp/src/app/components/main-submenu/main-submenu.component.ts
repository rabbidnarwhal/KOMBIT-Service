import { Component, OnInit } from '@angular/core';
import { EventsService } from 'src/app/services/events.service';

@Component({
  selector: 'app-main-submenu',
  templateUrl: './main-submenu.component.html',
  styleUrls: [ './main-submenu.component.scss' ]
})
export class MainSubmenuComponent implements OnInit {
  span = 6;
  submenu = [];
  selectedSubMenu = '';
  constructor(private eventsService: EventsService) {
    this.submenu = [
      { title: 'My Post', icon: 'assets/images/my-post.png', type: 'myPost' },
      { title: 'Favorite', icon: 'assets/images/favorite.png', type: 'favorite' },
      { title: 'Meeting', icon: 'assets/images/meeting.png', type: 'meeting' },
      { title: 'User', icon: 'assets/images/user.png', type: 'user' }
    ];
  }

  ngOnInit() {
    this.subscribeCloseModalState();
  }

  openModal(submenu: any) {
    this.selectedSubMenu = submenu.type;
    const content = {
      state: true,
      type: submenu.type,
      submenu: true,
      header: {
        icon: submenu.icon,
        text: submenu.title
      }
    };
    this.eventsService.setModalState(content);
  }

  subscribeCloseModalState() {
    this.eventsService.getModalState().subscribe((subs) => {
      if (!subs.state || !subs.submenu) {
        this.selectedSubMenu = '';
      }
    });
  }
}
