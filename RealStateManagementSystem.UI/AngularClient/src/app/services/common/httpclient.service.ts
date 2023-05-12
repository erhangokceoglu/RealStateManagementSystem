import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, Inject } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})

export class HttpclientService {

  constructor(private httpClient: HttpClient, @Inject('baseUrl') private baseUrl: string) { }

  private url(requestParameters: Partial<RequestParameters>): string {

    return `${requestParameters.baseUrl ? requestParameters.baseUrl : this.baseUrl}/${requestParameters.controller}/${requestParameters.action}`;
  }

  get<T>(requestParameters: Partial<RequestParameters>): Observable<T> {

    return this.httpClient.get<T>(this.getUrl(requestParameters), { headers: requestParameters.headers });
  }

  getEntity<T>(requestParameters: Partial<RequestParameters>, id? : number): Observable<T> {

    return this.httpClient.get<T>(this.getUrl(requestParameters) + `/${id}`, { headers: requestParameters.headers });
  }

  post<T>(requestParameters: Partial<RequestParameters>, body: Partial<T>): Observable<T> {

    return this.httpClient.post<T>(this.getUrl(requestParameters), body, { headers: requestParameters.headers })
  }

  put<T>(requestParameters: Partial<RequestParameters>, body: Partial<T>): Observable<T> {

    return this.httpClient.put<T>(this.getUrl(requestParameters), body, { headers: requestParameters.headers });
  }

  delete<T>(requestParameters: Partial<RequestParameters>, id: number): Observable<T> {

    return this.httpClient.delete<T>(this.getUrl(requestParameters) + `/${id}`, { headers: requestParameters.headers });
  }

  // async get<T>(requestParameters: Partial<RequestParameters>): Promise<T> {
  //   return await this.httpClient.get<T>(this.getUrl(requestParameters), { headers: requestParameters.headers }).toPromise();

  // }
  
  // async post<T>(requestParameters: Partial<RequestParameters>, body: Partial<T>): Promise<T> {
  //   return await this.httpClient.post<T>(this.getUrl(requestParameters), body, { headers: requestParameters.headers }).toPromise();

  // }
  
  // async put<T>(requestParameters: Partial<RequestParameters>, body: Partial<T>): Promise<T> {
  //   return await this.httpClient.put<T>(this.getUrl(requestParameters), body, { headers: requestParameters.headers }).toPromise();

  // }
  
  // async delete<T>(requestParameters: Partial<RequestParameters>, id: number): Promise<T> {
  //   return await this.httpClient.delete<T>(this.getUrl(requestParameters) + `/${id}`, { headers: requestParameters.headers }).toPromise();

  // }

  private getUrl(requestParameters: RequestParameters, id? : number, ): string {
    let url: string = '';
    if (requestParameters.fullEndpoint) {
      url = requestParameters.fullEndpoint;
    } else {
      url = `${this.url(requestParameters)}${id ? `/${id}` : ''}${requestParameters.queryString ? `?${requestParameters.queryString}` : ''}`;
    }
    return url;
  }
}

export class RequestParameters {
  controller?: string;
  action?: string;
  queryString?: string;
  headers?: HttpHeaders;
  baseUrl?: string;
  fullEndpoint?: string;
}
