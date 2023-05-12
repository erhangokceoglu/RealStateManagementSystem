import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ListappuserComponent } from './listappuser.component';

describe('ListappuserComponent', () => {
  let component: ListappuserComponent;
  let fixture: ComponentFixture<ListappuserComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ListappuserComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ListappuserComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
