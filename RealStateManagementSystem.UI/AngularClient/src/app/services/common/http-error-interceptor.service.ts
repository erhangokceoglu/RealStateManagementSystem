import { HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { tap } from "rxjs/operators";
import { Customtoastrservice, ToastrMessageType, ToastrPosition } from '../customtoastr.service';

@Injectable({
  providedIn: 'root'
})
export class HttpErrorInterceptorService implements HttpInterceptor {

  constructor(private toastrService : Customtoastrservice) { }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req).pipe(
      tap(
        succses => {},
        error => {
            if(error.status == 400){
              this.toastrService.message("Geçersiz istek yapıldı.", "Hata",{
                messageType : ToastrMessageType.Error,
                position : ToastrPosition.TopRight
              })
            }
            else if(error.status == 401){
              this.toastrService.message("Yetkisiz işlem yapıldı.", "Hata",{
                messageType : ToastrMessageType.Error,
                position : ToastrPosition.TopRight
              })
            }
            else if(error.status == 404){
              this.toastrService.message("İstek bulunamadı.", "Hata",{
                messageType : ToastrMessageType.Error,
                position : ToastrPosition.TopRight
              })
            }
            else if(error.status == 500){
              this.toastrService.message("Sunucu hatası oluştu.", "Hata",{
                messageType : ToastrMessageType.Error,
                position : ToastrPosition.TopRight
              })
            }
            else if(error.status == 503){
              this.toastrService.message("Servis hatası oluştu.", "Hata",{
                messageType : ToastrMessageType.Error,
                position : ToastrPosition.TopRight
              })
            }
            else if(error.status == 504){
              this.toastrService.message("Servis zaman aşımına uğradı.", "Hata",{
                messageType : ToastrMessageType.Error,
                position : ToastrPosition.TopRight
              })
            }
            else{
              this.toastrService.message("Bilinmeyen bir hata oluştu.", "Hata",{
                messageType : ToastrMessageType.Error,
                position : ToastrPosition.TopRight
            })
          }
        }
      )
    );
  }
}
