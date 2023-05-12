import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppusersComponent } from './appusers.component';
import { AddappuserModule } from './addappuser/addappuser.module';
import { ListappuserModule } from './listappuser/listappuser.module';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { DeleteDirective } from 'src/app/directives/common/delete.directive';
import { MatDialogModule } from '@angular/material/dialog';
import { DialogModule } from 'src/app/dialogs/dialog.module';
import { ReactiveFormsModule } from '@angular/forms';
import { MatCheckboxModule, MatFormFieldModule, MatOptionModule, MatSelectModule } from '@angular/material';
import { SharedDeleteModule } from 'src/app/dialogs/directives/common/shared-module/sharedDelete.module';

@NgModule({
  declarations: [AppusersComponent],
  imports: [
    CommonModule,
    AddappuserModule,
    ListappuserModule,
    MatTableModule,
    MatPaginatorModule,
    MatDialogModule,
    DialogModule,
    ReactiveFormsModule,
    MatCheckboxModule,
    MatSelectModule,
    MatFormFieldModule,
    MatOptionModule,
    SharedDeleteModule
  ],
})
export class AppusersModule { }