import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovedBusinessComponent } from './approved-business.component';

describe('ApprovedBusinessComponent', () => {
  let component: ApprovedBusinessComponent;
  let fixture: ComponentFixture<ApprovedBusinessComponent>;  

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApprovedBusinessComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovedBusinessComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
