import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { BranchesComparisionComponent } from './branches-comparision.component';

describe('BranchesComparisionComponent', () => {
  let component: BranchesComparisionComponent;
  let fixture: ComponentFixture<BranchesComparisionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ BranchesComparisionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(BranchesComparisionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
