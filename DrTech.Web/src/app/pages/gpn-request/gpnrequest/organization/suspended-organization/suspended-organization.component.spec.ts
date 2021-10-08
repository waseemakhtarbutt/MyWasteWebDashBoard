import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SuspendedOrganizationComponent } from './suspended-organization.component';

describe('SuspendedOrganizationComponent', () => {
  let component: SuspendedOrganizationComponent;
  let fixture: ComponentFixture<SuspendedOrganizationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SuspendedOrganizationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SuspendedOrganizationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
