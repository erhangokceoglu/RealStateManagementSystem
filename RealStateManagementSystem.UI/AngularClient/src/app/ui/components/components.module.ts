import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComponentsComponent } from './components.component';
import { HomeModule } from './home/home.module';
import { UserrealstatesModule } from './userrealstates/userrealstates.module';
import { LoginComponent } from './login/login.component';
import { LoginModule } from './login/login.module';
import { RouterModule } from '@angular/router';

@NgModule({
  declarations: [ComponentsComponent],
  imports: [
    CommonModule,
    HomeModule,
    UserrealstatesModule,
    LoginModule,
  ],
  exports: [ComponentsComponent]
})
export class ComponentsModule { }
