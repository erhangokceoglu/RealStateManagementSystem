import { Inject, Injectable } from '@angular/core';
import { HttpclientService } from '../../common/httpclient.service';
import { CreateForAppUserDto } from 'src/app/contracts/CreateForAppUserDto';
import { HttpErrorResponse } from '@angular/common/http';
import { ListAppUserDto } from 'src/app/contracts/ListAppUserDto';
import { Customtoastrservice, ToastrMessageType, ToastrPosition } from '../../customtoastr.service';
import { AuthService } from '../../common/auth.service';
import { ActivatedRoute, Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AppuserService {

  constructor(private httpClientService: HttpclientService, private toastrService : Customtoastrservice, private autService : AuthService, private router : Router, private activetedRoute :  ActivatedRoute, @Inject('baseUrl') private baseUrl: string) { }


    createAppUser(createForAppUserDto: CreateForAppUserDto)
    { 
      return this.httpClientService.post({
      controller: 'AppUser',
      action: 'Register',
      }, createForAppUserDto);
    }

    async readAppUsers(page: number = 0, size : number = 5, successCallback? : ()=> void, errorCallback?: (errorMessage: string) => void) : Promise<{totalCount : number ,listAppUsers : ListAppUserDto[]}> 
    {
      const promiseData : Promise<{totalCount : number ,listAppUsers : ListAppUserDto[]}> = this.httpClientService.get<{totalCount : number ,listAppUsers : ListAppUserDto[]}>(
      {
        controller: 'AppUser',
        action: 'GetAllUsers',
        queryString: `page=${page}&size=${size}`
      }).toPromise();
      promiseData
      return await promiseData;
    }

    deleteAppUser(id : number) 
    {
      this.httpClientService.delete(
      {
        controller: 'AppUser',
        action: 'Delete',
      }, id).subscribe();
    }

    getRoles()
    {
      return this.httpClientService.get(
      {
        controller: 'AppUser',
        action: 'GetRoles',
      }).pipe();
    }

    getAppUser(id : number)
    {
      return this.httpClientService.getEntity(
      {
        controller: 'AppUser',
        action: 'GetAppUser',
      },id).pipe();
    }

    updateAppUser(id : number, updateForAppUserDto : CreateForAppUserDto)
    {
      return this.httpClientService.put(
        {
          fullEndpoint: this.baseUrl+`/AppUser/Update/${id}`,
        },
        updateForAppUserDto
      ).pipe();
    }

    logout()
    {
      return this.httpClientService.post(
      {
        controller: 'AppUser',
        action: 'Logout',
      },null).pipe();
    }
    
    login(loginForAppUserDto)  
    { 
       return this.httpClientService.post(
      {
        controller: 'AppUser',
        action: 'Login',
      }, loginForAppUserDto).subscribe(
        (res:any)=>
        {
          if (res.token) 
          {
            localStorage.setItem('token',res.token);
            this.toastrService.message("Giriş Başarılı", "Başarı",
            {
              messageType: ToastrMessageType.Success,
              position: ToastrPosition.TopRight,
            },),
            (error : HttpErrorResponse) => {
              if(error.status == 400|| error.status == 500 || error.status == 404 || error.status == 401)
              {
                this.toastrService.message("Giriş Başarısız", "Hata",
                {
                  messageType: ToastrMessageType.Error,
                  position: ToastrPosition.TopRight,
                },)
              }
              else
              console.log(error);
            }
          }
          this.activetedRoute.queryParams.subscribe(params =>
          {
            const returnUrl : string = params['returnUrl'];
            if (returnUrl) 
            {
              this.router.navigate([returnUrl]);        
            }else
            {
              this.router.navigate(["/sistemyoneticisi/appusers"]);
            }
          }
        );
       this.autService.identityCheck();
      }
    );
  }
}

