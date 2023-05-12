import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AppusersComponent } from './appusers.component';

describe('AppusersComponent', () => {
  let component: AppusersComponent;
  let fixture: ComponentFixture<AppusersComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AppusersComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AppusersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
