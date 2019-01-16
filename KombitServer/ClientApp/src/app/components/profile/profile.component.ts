import { Component, OnInit, Input } from '@angular/core';
import { AuthService } from 'src/app/services/auth.service';
import { EventsService } from 'src/app/services/events.service';
import { ProfileService } from 'src/app/services/profile.service';
import { NzMessageService } from 'ng-zorro-antd';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: [ './profile.component.scss' ]
})
export class ProfileComponent implements OnInit {
  @Input() header: any;
  profile: any = {};
  isLoading = true;
  mapImage = '';
  mapUrl = '';
  picture = '';
  constructor(
    private authService: AuthService,
    private eventsService: EventsService,
    private profileService: ProfileService,
    private messageService: NzMessageService
  ) {}

  ngOnInit() {
    this.getProfile();
  }

  logout() {
    this.authService.logout();
    this.closeModal();
  }

  closeModal() {
    const content = {
      state: false,
      type: ''
    };
    this.eventsService.setModalState(content);
  }

  async getProfile() {
    try {
      this.profile = await this.profileService.getProfile();
      this.isLoading = false;
      this.picture = this.profile.image ? this.profile.image : 'assets/images/profile.png';
      this.createMap();
    } catch (error) {
      this.messageService.error(error);
    }
  }

  createMap() {
    const position = this.profile.addressKoordinat ? this.profile.addressKoordinat.split(', ') : [];
    if (position.length) {
      this.mapImage =
        'https://maps.googleapis.com/maps/api/staticmap?center=' +
        position[0] +
        ',' +
        position[1] +
        '&markers=color:orange%7Clabel:C%7C' +
        position[0] +
        ',' +
        position[1] +
        '&zoom=16&size=540x324&key=' +
        environment.GOOGLE_MAP_API_KEY;

      // tslint:disable-next-line:max-line-length
      this.mapUrl = `https://www.google.com/maps/place/${position[0]},${position[1]}/@${position[0]},${position[1]}/data=!3m1!4b1!4m6!3m5!1s0x0:0x0!7e2!8m2!3d${position[0]}!4d${position[1]}`;
    }
  }
}
