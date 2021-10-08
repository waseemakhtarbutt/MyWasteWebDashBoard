import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SuspendedBusinessComponent } from './suspended-business.component';

describe('SuspendedBusinessComponent', () => {
  let component: SuspendedBusinessComponent;
  let fixture: ComponentFixture<SuspendedBusinessComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SuspendedBusinessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SuspendedBusinessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
