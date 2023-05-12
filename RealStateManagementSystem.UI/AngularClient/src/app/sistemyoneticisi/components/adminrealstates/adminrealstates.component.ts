import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { CreateRealStateDto } from 'src/app/contracts/CreateRealStateDto';
import { ListRealStateDto } from 'src/app/contracts/ListRealStateDto';
import { Customtoastrservice, ToastrMessageType, ToastrPosition } from 'src/app/services/customtoastr.service';
import { RealstateService } from 'src/app/services/sistemyoneticisi/models/realstate.service';
import * as XLSX from 'xlsx';

@Component({
  selector: 'app-adminrealstates',
  templateUrl: './adminrealstates.component.html',
  styleUrls: ['./adminrealstates.component.scss']
})
export class AdminrealstatesComponent implements OnInit {

  constructor(private realStateService: RealstateService, private toastrService :  Customtoastrservice, private formBuilder: FormBuilder) { }

  displayedColumns: string[] = ['id', 'province', 'district', 'neighbourhood', 'islandNo', 'parcelNo', 'qualification' , 'address', 'edit' , 'delete'];
  dataSource :  MatTableDataSource<ListRealStateDto> = null;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  editMode : boolean = false;
  submitted : boolean = false; 
  currentRealStateId : number = 0;
  frm : FormGroup
  provinces:{};
  districts:{};
  neighbourhoods:{};
  qualifications:{};
  fileName = 'Tasinmazlar.xlsx';
  rows : any = [];

  async ngOnInit() 
  {
    this.frm = this.formBuilder.group
    (
      {
      provinces: ['', [Validators.required]],
      districts: ['', [Validators.required]],
      neighbourhoods: ['', [Validators.required]],
      islandNo: ['', [Validators.required, Validators.maxLength(100), Validators.pattern('^[0-9]+$')]],
      parcelNo: ['', [Validators.required, Validators.maxLength(100), Validators.pattern('^[0-9]+$')]],
      qualifications: ['', [Validators.required]],
      address: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(180)]],
      }
    );
    await this.getRealStates();
    this.realStateService.getProvinces().subscribe(
    data => this.provinces = data
    );
    this.qualifications = this.realStateService.getQualifications();
  }

  get f() { return this.frm.controls; }

  onSubmit() 
  { 
    this.submitted = true;
    if (this.frm.invalid) 
    {
    return;
    }

    let createRealStateDto : CreateRealStateDto = 
    {
    provinceId: this.f.provinces.value,
    districtId: this.f.districts.value,
    neighbourhoodId: this.f.neighbourhoods.value,
    islandNo: this.f.islandNo.value.trim(),
    parcelNo: this.f.parcelNo.value.trim(),
    qualification: this.f.qualifications.value,
    address: this.f.address.value.trim(),
    createDate: new Date(Date.now()),
    isActive: true,
    };

    if(!this.editMode)
    {
      this.realStateService.createRealState(createRealStateDto).subscribe(
       (res: any)=>
        {
          this.toastrService.message("Taşınmaz Kayıtı Başarılı.", "Başarılı",
          {
          messageType: ToastrMessageType.Success,
          position: ToastrPosition.TopRight,
          });
          this.createdRealState.emit(createRealStateDto);
          this.pageChanged();
          this.frm.reset();
          this.frm.controls.provinces.setValue('');
          this.frm.controls.districts.setValue('');
          this.frm.controls.neighbourhoods.setValue('');
          this.frm.controls.qualifications.setValue('');
          this.submitted = false;
        },
        (error : any) => 
        {
          this.toastrService.message("Taşınmaz kayıtı başarısız.", "Başarısız",
          {
            messageType: ToastrMessageType.Error,
            position: ToastrPosition.TopRight,
          });
        }
      );   
    }
    else
    {
      this.realStateService.updateRealState(this.currentRealStateId, createRealStateDto).subscribe
      (
        
       (res: any)=>
        {
          this.toastrService.message("Taşınmaz Güncelleme Başarılı.", "Başarılı",
          {
          messageType: ToastrMessageType.Success,
          position: ToastrPosition.TopRight,
          });
          this.pageChanged();
          this.frm.reset();
          this.frm.controls.provinces.setValue('');
          this.frm.controls.districts.setValue('');
          this.frm.controls.neighbourhoods.setValue('');
          this.frm.controls.qualifications.setValue('');
          this.submitted = false;
          this.editMode = false;
        },
        (error : any) =>
        {
          this.toastrService.message("Taşınmaz Güncelleme Başarısız.", "Başarısız",
          {
          messageType: ToastrMessageType.Error,
          position: ToastrPosition.TopRight,
          });
        }
      );
    }
  }
  district : any;

  onEdit(id: number)
  {
    this.currentRealStateId = id;
    this.realStateService.getRealState(id).subscribe(
      (res: any)=>
      {
        this.frm.controls.provinces.setValue(res.provinceId);
        this.frm.controls.districts.setValue(res.districtId);
        this.realStateService.getDistricts(res.provinceId).subscribe(
          data => this.districts = data
        );
        this.realStateService.getNeighbourhoods(res.districtId).subscribe(
          data => this.neighbourhoods = data
        );
        this.frm.controls.neighbourhoods.setValue(res.neighbourhoodId);
        this.frm.controls.islandNo.setValue(res.islandNo);
        this.frm.controls.parcelNo.setValue(res.parcelNo);
        this.frm.controls.qualifications.setValue(res.qualification);
        this.frm.controls.address.setValue(res.address);
        this.editMode = true;
        this.toastrService.message(`${res.islandNo}/${res.parcelNo} Ada Parselin Bilgileri İlgili Alanlara Başarılı Gelmiştir.`, "Başarılı",
        {
          messageType: ToastrMessageType.Success,
          position: ToastrPosition.TopRight,
        });
      }
    );
  } 

  onElementSelected(event,element: ListRealStateDto) 
  {
    debugger;
    if (event.checked) 
    {
      this.rows.push(element);
      this.toastrService.message(element.islandNo+"/"+element.parcelNo+" seçildi.", "Uyarı",
      {
        messageType: ToastrMessageType.Warning,
        position: ToastrPosition.TopRight,
      });
    }
    else 
    {
      let index = this.rows.findIndex(x => x.id == element.id);
      this.rows.splice(index, 1);
    }
  }

  async getRealStates() 
  {
    const allRealStates : {totalCount : number ,listRealStates : ListRealStateDto[]} = await this.realStateService.readRealStates(this.paginator ? this.paginator.pageIndex : 0, this.paginator ? this.paginator.pageSize : 5);
    this.dataSource = new MatTableDataSource<ListRealStateDto>(allRealStates.listRealStates);
    this.paginator.length = allRealStates.totalCount;
  }

  async pageChanged() 
  {
    await this.getRealStates();
  }

  getQualifications() 
  {
    return this.realStateService.getQualifications();
  }

  getProvinces() 
  {
    return this.realStateService.getProvinces();
  }

  onChangeProvinces(provinceId: string)
  {
    if(provinceId)
    {
      this.realStateService.getDistricts(parseInt(provinceId)).subscribe(
          data => 
          {
            this.frm.controls.districts.enable();
            this.frm.controls.districts.setValue('');
            this.districts = data;
          }
      );
    }
  }

  onChangeDistricts(districtId: string)
  {
    if(districtId)
    {
      this.realStateService.getNeighbourhoods(parseInt(districtId)).subscribe(
        data => 
        {
          this.frm.controls.neighbourhoods.enable();
          this.frm.controls.neighbourhoods.setValue('');
          this.neighbourhoods = data;
        }
      );
    }
  }

  @Output() createdRealState : EventEmitter<CreateRealStateDto> = new EventEmitter();
  @ViewChild('AdminrealstatesComponent') adminrealstatesComponent : AdminrealstatesComponent;

  createdAppUserHandler() 
  {
    this.adminrealstatesComponent;
  }

  successMessage() 
    {
      this.toastrService.message("Excel dosyası başarıyla indirildi.", "Başarılı",
      {
        messageType: ToastrMessageType.Success,
        position: ToastrPosition.TopRight,
      });
    }

  excel()
  {
    if(this.rows.length > 0)
    {
      const worksheet = XLSX.utils.json_to_sheet(this.rows);
      const workbook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(workbook, worksheet, 'Sheet1');
      XLSX.writeFile(workbook, this.fileName);
      this.rows = [];
      this.successMessage()
    }
    else
      {
      let element = document.getElementById('excel-table');
      const ws: XLSX.WorkSheet =XLSX.utils.table_to_sheet(element);
      const wb: XLSX.WorkBook = XLSX.utils.book_new();
      XLSX.utils.book_append_sheet(wb, ws, 'Sheet1');
      XLSX.writeFile(wb, this.fileName);   
      this.successMessage() 
    } 
    this.pageChanged();
  }

  filterData($event: any)
  {
    this.dataSource.filter = $event.target.value.toString().trim().toLowerCase();
  }
}


