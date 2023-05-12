import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/common/auth.service';
import { Customtoastrservice, ToastrMessageType, ToastrPosition } from 'src/app/services/customtoastr.service';
import { AppuserService } from 'src/app/services/sistemyoneticisi/models/appuser.service';

@Component({
  selector: 'app-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.scss']
})
export class SidebarComponent implements OnInit {

  constructor(public authService : AuthService, private toastrService : Customtoastrservice, private router : Router, private appuserService : AppuserService) { }

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
  )};

}
