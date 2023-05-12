import { Inject, Injectable } from '@angular/core';
import { CreateRealStateDto, Qualification } from 'src/app/contracts/CreateRealStateDto';
import { HttpclientService } from '../../common/httpclient.service';
import { ListRealStateDto } from 'src/app/contracts/ListRealStateDto';
@Injectable({
  providedIn: 'root'
})
export class RealstateService {

  constructor(private httpClientService : HttpclientService, @Inject('baseUrl') private baseUrl: string) { }


  createRealState(createRealStateDto: CreateRealStateDto)
  { 
    return this.httpClientService.post({
      controller: 'RealState',
      action: 'Add',
    }, createRealStateDto);
  }

  async readRealStates(page: number = 0, size : number = 5, successCallback? : ()=> void, errorCallback?: (errorMessage: string) => void) : Promise<{totalCount : number ,listRealStates : ListRealStateDto[]}> 
  {
    const promiseData : Promise<{totalCount : number ,listRealStates : ListRealStateDto[]}> = this.httpClientService.get<{totalCount : number , listRealStates : ListRealStateDto[]}>(
    {
      controller: 'RealState',
      action: 'GetAll',
      queryString: `page=${page}&size=${size}`
    }).toPromise();
    promiseData
    return await promiseData;
  }

  deleteRealState(id : number) 
  {
    this.httpClientService.delete(
    {
      controller: 'RealState',
      action: 'Delete',
    }, id).subscribe();
  }

  updateRealState(id: number, updateRealStateDto: CreateRealStateDto)
  {
    return this.httpClientService.put(
      {
        fullEndpoint: this.baseUrl+`/RealState/Update/${id}`,
      },
      updateRealStateDto
    ).pipe();
  }

  getRealState(id : number)
  {
    return this.httpClientService.getEntity(
    {
      controller: 'RealState',
      action: 'GetById',
    },id).pipe();
  }

  getProvinces()
  {
    return this.httpClientService.get(
    {
      controller: 'RealState',
      action: 'GetAllProvinces',
    }).pipe();
  }

  getQualifications()
  {
    return[
      {id: 1, name: Qualification.Land},
      {id: 2, name: Qualification.Residential},
      {id: 3, name: Qualification.Field}
    ]
  }

  getDistricts(provienceId: number)
  {
    return this.httpClientService.getEntity(
    {
      controller: 'RealState',
      action: 'GetAllDistricts',
    },provienceId).pipe();
  }

  getNeighbourhoods(districtId: number)
  {
    return this.httpClientService.getEntity(
    {
      controller: 'RealState',
      action: 'GetAllNeighbourhoods',
    },districtId).pipe();
  }
}
