import { TestBed } from '@angular/core/testing';

import { MywasteserviceService } from './mywasteservice.service';

describe('MywasteserviceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: MywasteserviceService = TestBed.get(MywasteserviceService);
    expect(service).toBeTruthy();
  });
});
