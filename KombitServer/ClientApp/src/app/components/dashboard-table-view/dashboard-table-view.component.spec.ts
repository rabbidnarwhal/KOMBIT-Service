import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DashboardTableViewComponent } from './dashboard-table-view.component';

describe('DashboardTableViewComponent', () => {
  let component: DashboardTableViewComponent;
  let fixture: ComponentFixture<DashboardTableViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DashboardTableViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DashboardTableViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
