import { Injectable } from '@angular/core';
import { DialogPosition, MatDialog } from '@angular/material/dialog';
import { ComponentType } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class DialogService {

  constructor(private dialog : MatDialog) { }

  openDialog(dialogParameters : Partial<DialogParameters>): void {
    const dialogRef = this.dialog.open(dialogParameters.componentType, {
      width: dialogParameters.dialogOptions? dialogParameters.dialogOptions.width : '350px',
      height: dialogParameters.dialogOptions? dialogParameters.dialogOptions.height : '250px',
      position : dialogParameters.dialogOptions ? dialogParameters.dialogOptions.position : null,
      data: dialogParameters.data
    });

    dialogRef.afterClosed().subscribe(result => {
      if(result == dialogParameters.data) {
        dialogParameters.afterClosed();
      }});
  }
}
 export class DialogParameters{
  componentType :  ComponentType<any>;
  data : any;
  afterClosed : () => void;
  dialogOptions? : Partial<DialogOptions> = new DialogOptions();
 }

 export class DialogOptions{
  width? : string = '350px';
  height? : string;
  position? : DialogPosition;
}
