import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SuspendedSchoolComponent } from './suspended-school.component';

describe('SuspendedSchoolComponent', () => {
  let component: SuspendedSchoolComponent;
  let fixture: ComponentFixture<SuspendedSchoolComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SuspendedSchoolComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SuspendedSchoolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
