import { Injectable } from '@angular/core';
declare var alertify : any;

@Injectable({
  providedIn: 'root'
})


export class AlertifyService {
  
  constructor() { }
  
  message(message: string, options?: Partial<AlertifyOptions>) {
    if(options.timeOut != null) {
      alertify.set('notifier','delay', options.timeOut);
    }
    alertify.set('notifier','position', options.position);
    if (options.dismissOther != null) {
      alertify.set('notifier','dismissOther', options.dismissOther);
    }
    alertify[options.messageType](message);
  }  
}

export enum AlertityMessageType {
  Success = "success",
  Error = "error",
  Warning = "warning",
  notify = "notify",
  message = "message"
}

export enum Position {
  TopRight = "top-right",
  TopLeft = "top-left",
  BottomRight = "bottom-right",
  BottomLeft = "bottom-left",
  TopCenter = "top-center",
  BottomCenter = "bottom-center"
}

export class AlertifyOptions {
  message: string;
  messageType: AlertityMessageType = AlertityMessageType.Success;
  position: Position = Position.TopRight;
  timeOut?: number = 3;
  dismissOther?: boolean = true;
}
