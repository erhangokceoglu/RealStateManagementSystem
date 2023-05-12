import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, Router } from '@angular/router';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Observable } from 'rxjs';
import { _isAuth } from 'src/app/services/common/auth.service';
import { Customtoastrservice, ToastrMessageType, ToastrPosition } from 'src/app/services/customtoastr.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(private router: Router, private toastrService : Customtoastrservice){

  }

  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean> | Promise<boolean> | boolean {

      if(!_isAuth){
        this.router.navigate(["login"], {queryParams: {returnUrl: state.url}});
        this.toastrService.message("Lütfen giriş yapınız.", "Uyarı",{
          messageType : ToastrMessageType.Warning,
          position : ToastrPosition.TopRight,
        });
      }
      return true;
  }
}
