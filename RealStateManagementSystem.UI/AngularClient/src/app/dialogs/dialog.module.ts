import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DeletedialogComponent } from './deletedialog/deletedialog.component';
import { MatDialogModule } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';

@NgModule({
  declarations: [ DeletedialogComponent ],
  imports: [
    CommonModule,
    MatDialogModule,
    MatButtonModule
  ],
  exports: [ DeletedialogComponent ]
})
export class DialogModule { }
