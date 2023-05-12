import { Component } from '@angular/core';
import { AuthService } from './services/common/auth.service';
import { Customtoastrservice, ToastrMessageType, ToastrPosition } from './services/customtoastr.service';
import { Router } from '@angular/router';
import { AppuserService } from './services/sistemyoneticisi/models/appuser.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
        constructor(public authService : AuthService, private toastrService : Customtoastrservice, private router : Router, private appuserService : AppuserService) {
          this.authService.identityCheck();
        }

        ngOnInit() {         
        }

        signOut(){
          this.appuserService.logout().subscribe();
          localStorage.removeItem('token');
          this.authService.identityCheck();
          this.router.navigate([""]);
          this.toastrService.message("Çıkış yapıldı.", "Bilgi",{
            messageType : ToastrMessageType.Info,
            position : ToastrPosition.TopRight
          }
        );
      }  
}
