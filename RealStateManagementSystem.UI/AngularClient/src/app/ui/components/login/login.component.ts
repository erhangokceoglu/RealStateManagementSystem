import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/services/common/auth.service';
import { AppuserService } from 'src/app/services/sistemyoneticisi/models/appuser.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private appUserService : AppuserService, private autService : AuthService, private activetedRoute : ActivatedRoute, private router : Router) { }

  formModel = 
  {
    Email:'',
    Password:''
  }

  ngOnInit() {

  }

  OnSubmit(form:NgForm){
    this.appUserService.login(form.value);
  }
}
