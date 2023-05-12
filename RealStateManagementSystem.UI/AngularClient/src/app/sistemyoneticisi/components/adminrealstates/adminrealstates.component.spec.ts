import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdminrealstatesComponent } from './adminrealstates.component';

describe('AdminrealstatesComponent', () => {
  let component: AdminrealstatesComponent;
  let fixture: ComponentFixture<AdminrealstatesComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdminrealstatesComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdminrealstatesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
