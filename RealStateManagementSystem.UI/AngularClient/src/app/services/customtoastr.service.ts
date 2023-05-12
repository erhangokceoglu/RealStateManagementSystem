import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

@Injectable({
  providedIn: 'root'
})
export class Customtoastrservice {

  constructor(private toastr: ToastrService) {}
    message(message: string, title: string, toastrOption: Partial<ToastrOption>){
        this.toastr[toastrOption.messageType](message, title, {
            positionClass: toastrOption.position
        });
   }
}

export class ToastrOption {
    message: string;
    title: string;
    messageType: ToastrMessageType;
    position: ToastrPosition;
}

export enum ToastrMessageType {
    Success = 'success',
    Error = 'error',
    Warning = 'warning',
    Info = 'info'
}


export enum ToastrPosition {
    TopRight = 'toast-top-right',
    TopLeft = 'toast-top-left',
    TopCenter = 'toast-top-center',
    BottomRight = 'toast-bottom-right',
    BottomLeft = 'toast-bottom-left',
    BottomCenter = 'toast-bottom-center',
    BottomFullWidth = 'toast-bottom-full-width',
    TopFullWidth = 'toast-top-full-width',
    CenterCenter = 'toast-center-center'
}