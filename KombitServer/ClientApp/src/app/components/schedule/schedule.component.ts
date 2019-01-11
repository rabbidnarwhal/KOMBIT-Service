import { Component, OnInit, Input } from '@angular/core';
import { Subject } from 'rxjs';
import { CalendarEvent, CalendarEventAction } from 'angular-calendar';
import { startOfDay, endOfDay, subDays, addDays, endOfMonth, isSameDay, isSameMonth, addHours } from 'date-fns';
import { ScheduleService } from 'src/app/services/schedule.service';
import { EventsService } from 'src/app/services/events.service';

const monthNames = [ 'Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun', 'Jul', 'Aug', 'Sep', 'Oct', 'Nov', 'Dec' ];

@Component({
  selector: 'app-schedule',
  templateUrl: './schedule.component.html',
  styleUrls: [ './schedule.component.scss' ]
})
export class ScheduleComponent implements OnInit {
  @Input() header: any;

  isLoading = true;
  viewDate: Date = new Date();
  events: CalendarEvent[] = [
    {
      start: startOfDay('12-31-2018'),
      title: 'An event with no end date'
    }
  ];
  activeDayIsOpen = false;

  constructor(private scheduleService: ScheduleService, private eventsService: EventsService) {}

  ngOnInit() {
    this.getEventData();
  }

  async getEventData() {
    const schedule = await this.scheduleService.getSchedules();
    if (schedule.length) {
      this.events = schedule.map((x) => {
        const time = new Date(x.date);
        time.setUTCHours(time.getUTCHours() + -time.getTimezoneOffset() / 60);
        const localTime = time.toLocaleString();
        const event = {
          start: startOfDay(localTime),
          title: `Meeting with ${x.userName}`,
          sid: x.id
        };
        return event;
      });
    }
    this.isLoading = false;
  }

  dayClicked(event: CalendarEvent[]): void {
    const content = {
      state: true,
      type: 'meetingDetail',
      submenu: true,
      header: this.header,
      scheduleId: event[0]['sid']
    };
    this.eventsService.setModalState(content);
  }

  showMonth(date: Date) {
    if (date.getDate() === 1) {
      return monthNames[date.getMonth()];
    } else {
      return;
    }
  }
}
