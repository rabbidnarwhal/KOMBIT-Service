import { Component, OnInit, Input } from '@angular/core';
import { ScheduleService } from 'src/app/services/schedule.service';
import { AuthService } from 'src/app/services/auth.service';

@Component({
  selector: 'app-schedule-detail',
  templateUrl: './schedule-detail.component.html',
  styleUrls: [ './schedule-detail.component.scss' ]
})
export class ScheduleDetailComponent implements OnInit {
  @Input() sid: number;
  @Input() header: any;
  schedule: any = {};
  isLoading = true;
  constructor(private scheduleService: ScheduleService) {}

  ngOnInit() {
    this.getScheduleDetail();
  }

  async getScheduleDetail() {
    this.schedule = await this.scheduleService.getScheduleDetail(this.sid);
    const time = new Date(this.schedule.date);
    time.setUTCHours(time.getUTCHours() + -time.getTimezoneOffset() / 60);
    this.schedule.date = time.toLocaleString();
    this.schedule.userName = this.schedule.recepientName;
    this.isLoading = false;
    // this.createMap();
  }
}
