import { HttpErrorResponse } from '@angular/common/http';
import { Directive, ElementRef, EventEmitter, HostListener, Input, Output, Renderer2} from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { DeleteState, DeletedialogComponent } from 'src/app/dialogs/deletedialog/deletedialog.component';
import { AlertifyService, AlertityMessageType, Position } from 'src/app/services/alertify.service';
import { DialogService } from 'src/app/services/common/dialog.service';
import { HttpclientService } from 'src/app/services/common/httpclient.service';
import { Customtoastrservice, ToastrMessageType, ToastrPosition } from 'src/app/services/customtoastr.service';
declare var $: any;

@Directive({
  selector: '[appDelete]'
})
export class DeleteDirective {

  constructor(private element: ElementRef, private renderer : Renderer2, private httpClientService : HttpclientService, public dialog: MatDialog, private toastrService : Customtoastrservice, private dialogService : DialogService) {
    const img =  renderer.createElement("img");
    img.setAttribute('src', '../../../../assets/delete.png');
    img.setAttribute('style', 'width: 30px; height: 30px; cursor: pointer;');
    renderer.appendChild(element.nativeElement, img);
   }

   @Input() id: number;
   @Input() controller: string;
   @Input() action: string;
   @Output() callBack :  EventEmitter<any> = new EventEmitter();

   @HostListener('click') onClick() {
    this.dialogService.openDialog({
      componentType : DeletedialogComponent,
      data : DeleteState.Yes,
      afterClosed : () => {
        const td : HTMLTableCellElement = this.element.nativeElement;
        this.httpClientService.delete({
          controller: this.controller,
          action: this.action,}, this.id).subscribe();
        $(td.parentElement).fadeOut(2000, () => {
          if(this.controller == "AppUser"){
            debugger;
            this.callBack.emit();
            this.toastrService.message("Kullanıcı Silme Başarılı", "Başarılı",{
              messageType: ToastrMessageType.Success,
              position: ToastrPosition.TopRight,
            });
          }else if(this.controller == "RealState"){
            this.callBack.emit();
            this.toastrService.message("Tasinmaz Silme Başarılı", "Başarılı",{
              messageType: ToastrMessageType.Success,
              position: ToastrPosition.TopRight,
            });
          }
        });
      }
    });
   }
}
