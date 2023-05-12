import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddappuserComponent } from './addappuser.component';

describe('AddappuserComponent', () => {
  let component: AddappuserComponent;
  let fixture: ComponentFixture<AddappuserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddappuserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddappuserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
