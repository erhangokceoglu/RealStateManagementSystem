import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UserrealstatesComponent } from './userrealstates.component';

describe('UserrealstatesComponent', () => {
  let component: UserrealstatesComponent;
  let fixture: ComponentFixture<UserrealstatesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UserrealstatesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UserrealstatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
