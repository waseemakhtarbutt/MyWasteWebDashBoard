import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ApprovedOrganizationComponent } from './approved-organization.component';

describe('ApprovedOrganizationComponent', () => {
  let component: ApprovedOrganizationComponent;
  let fixture: ComponentFixture<ApprovedOrganizationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ApprovedOrganizationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ApprovedOrganizationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
