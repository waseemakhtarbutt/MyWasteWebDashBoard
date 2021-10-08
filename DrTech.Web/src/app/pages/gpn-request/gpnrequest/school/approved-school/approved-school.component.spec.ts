import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovedSchoolComponent } from './approved-school.component';

describe('ApprovedSchoolComponent', () => {
  let component: ApprovedSchoolComponent;
  let fixture: ComponentFixture<ApprovedSchoolComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApprovedSchoolComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovedSchoolComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
