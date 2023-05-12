import { TestBed } from '@angular/core/testing';
import { Customtoastrservice } from './customtoastr.service';

describe('CustomtoastrserviceService', () => {
  beforeEach(() => TestBed.configureTestingModule({}));

  it('should be created', () => {
    const service: Customtoastrservice = TestBed.get(Customtoastrservice);
    expect(service).toBeTruthy();
  });
});
