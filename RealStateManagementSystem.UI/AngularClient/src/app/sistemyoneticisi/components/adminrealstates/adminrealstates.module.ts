import { NgModule } from '@angular/core';
import { AdminrealstatesComponent } from './adminrealstates.component';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { DialogModule } from 'src/app/dialogs/dialog.module';
import { MatTableModule } from '@angular/material/table';
import { MatPaginatorModule } from '@angular/material/paginator';
import { MatDialogModule } from '@angular/material/dialog';
import { MatCheckboxModule, MatFormFieldModule, MatOptionModule, MatSelectModule } from '@angular/material';
import { NgxSelectModule } from 'ngx-select-ex';
import { SharedDeleteModule } from 'src/app/dialogs/directives/common/shared-module/sharedDelete.module';

@NgModule({
  declarations: [AdminrealstatesComponent],
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatPaginatorModule,
    MatDialogModule,
    DialogModule,
    MatCheckboxModule,
    MatSelectModule,
    MatFormFieldModule,
    MatOptionModule,
    NgxSelectModule,
    SharedDeleteModule
  ]
})
export class AdminrealstatesModule { }
