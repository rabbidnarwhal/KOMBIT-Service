import { Injectable } from '@angular/core';
import { Subject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class EventsService {
  eventModal: Subject<any> = new Subject();
  eventSearchText: Subject<any> = new Subject();
  constructor() {}

  setModalState(content: any) {
    this.eventModal.next(content);
  }

  setProductSearchText(searchText: string) {
    this.eventSearchText.next(searchText);
  }

  getModalState(): Observable<any> {
    return this.eventModal.asObservable();
  }

  getProductSearchText(): Observable<string> {
    return this.eventSearchText.asObservable();
  }
}
