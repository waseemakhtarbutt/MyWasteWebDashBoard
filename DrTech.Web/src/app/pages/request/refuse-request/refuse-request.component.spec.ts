import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { RefuseRequestComponent } from './refuse-request.component';

describe('RefuseRequestComponent', () => {
  let component: RefuseRequestComponent;
  let fixture: ComponentFixture<RefuseRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ RefuseRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(RefuseRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
