import { Injectable } from '@angular/core';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private jwtHelper : JwtHelperService) { }
  identityCheck(){
    const token = localStorage.getItem('token');
    let expired : boolean ;
    try {
      expired = this.jwtHelper.isTokenExpired(token);
    } catch (error)
    {
      expired = true;     
    }
    _isAuth = token != null && !expired
  }

  get isAuthenticated(): boolean{
    return _isAuth;
  }
}

export let _isAuth : boolean;