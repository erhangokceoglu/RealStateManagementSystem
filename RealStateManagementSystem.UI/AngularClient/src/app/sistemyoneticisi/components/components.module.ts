import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppusersModule } from './appusers/appusers.module';
import { LogsModule } from './logs/logs.module';
import { DashboardModule } from './dashboard/dashboard.module';
import { AdminrealstatesModule } from './adminrealstates/adminrealstates.module';

@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    AppusersModule,
    LogsModule,
    DashboardModule,
    AdminrealstatesModule
  ]
})
export class ComponentsModule { }
