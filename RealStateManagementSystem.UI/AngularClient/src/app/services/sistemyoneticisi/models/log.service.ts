import { Injectable } from '@angular/core';
import { ListLogDto } from 'src/app/contracts/ListLogDto';
import { HttpclientService } from '../../common/httpclient.service';
import { HttpErrorResponse } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class LogService {

  constructor(private httpClientService :  HttpclientService) { }

  async readLogs(page: number = 0, size : number = 5, successCallback? : ()=> void, errorCallback?: (errorMessage: string) => void) : Promise<{totalCount : number , listLogs : ListLogDto[]}> 
  {
    const promiseData : Promise<{totalCount : number ,listLogs : ListLogDto[]}> = 
    this.httpClientService.get<{totalCount : number ,listLogs : ListLogDto[]}>(
    {
      controller: 'Log',
      action: 'GetLogs',
      queryString: `page=${page}&size=${size}`
    }).toPromise();
    promiseData.then(d=> successCallback()).catch((errorResponse : HttpErrorResponse) => errorCallback(errorResponse.message))
    return await promiseData;
  }
}