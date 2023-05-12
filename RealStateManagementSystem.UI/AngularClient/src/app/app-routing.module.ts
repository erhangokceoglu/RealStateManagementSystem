import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { LayoutComponent } from './sistemyoneticisi/layout/layout.component';
import { AppusersComponent } from './sistemyoneticisi/components/appusers/appusers.component';
import { DashboardComponent } from './sistemyoneticisi/components/dashboard/dashboard.component';
import { LogsComponent } from './sistemyoneticisi/components/logs/logs.component';
import { AdminrealstatesComponent } from './sistemyoneticisi/components/adminrealstates/adminrealstates.component';
import { UserrealstatesComponent } from './ui/components/userrealstates/userrealstates.component';
import { LoginComponent } from './ui/components/login/login.component';
import { AuthGuard } from './guards/common/auth.guard';


const routes: Routes = [
  {
    path:'sistemyoneticisi',component: LayoutComponent,
    children:[
      {path:'',component: DashboardComponent, canActivate: [AuthGuard]},
      {path:'appusers',component: AppusersComponent, canActivate: [AuthGuard]},
      {path:'logs',component: LogsComponent, canActivate: [AuthGuard]},
      {path:'adminrealstates', component: AdminrealstatesComponent, canActivate: [AuthGuard]},
     ], canActivate: [AuthGuard]
    },
     {path:'', component: LoginComponent},  
    //  {path:'',component: HomeComponent},
     {path:'userrealstates', component: UserrealstatesComponent},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
