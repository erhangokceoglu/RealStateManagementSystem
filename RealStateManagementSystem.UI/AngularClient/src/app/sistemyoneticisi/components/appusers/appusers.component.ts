import { Component, EventEmitter, OnInit, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { CreateForAppUserDto } from 'src/app/contracts/CreateForAppUserDto';
import { ListAppUserDto } from 'src/app/contracts/ListAppUserDto';
import { Customtoastrservice, ToastrMessageType, ToastrPosition } from 'src/app/services/customtoastr.service';
import { AppuserService } from 'src/app/services/sistemyoneticisi/models/appuser.service';
import * as XLSX from 'xlsx';
declare var $: any;

@Component({
  selector: 'app-appusers',
  templateUrl: './appusers.component.html',
  styleUrls: ['./appusers.component.scss']
})
export class AppusersComponent implements OnInit{

  constructor(public appuserService : AppuserService, private toastrService : Customtoastrservice, private formBuilder : FormBuilder) { }

  displayedColumns: string[] = ['id', 'name', 'surname', 'email', 'role', 'address', 'edit' , 'delete'];
  dataSource :  MatTableDataSource<ListAppUserDto> = null;
  @ViewChild(MatPaginator) paginator: MatPaginator;
  editMode : boolean = false;
  submitted : boolean = false; 
  currentAppUserId : number = 0;
  roles:{};
  frm : FormGroup
  fileName = 'Kullanicilar.xlsx';
  rows : any = [];

  async ngOnInit() 
  {
    this.frm = this.formBuilder.group(
    {
      name: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(100), Validators.pattern('^[a-zA-Z]+$')]],
      surname: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(100), Validators.pattern('^[a-zA-Z]+$')]],
      email: ['', [Validators.required, Validators.minLength(10), Validators.maxLength(100) , Validators.email]],
      password : ['', [Validators.required, Validators.minLength(8), Validators.maxLength(100), 
      Validators.pattern(/^(?=.*[A-Za-z])(?=.*\d)(?=.*[@$!%*#?&.+:,])[A-Za-z\d@$!%*#?&.+:,]{8,}$/)]],
      address: ['', [Validators.required, Validators.minLength(5), Validators.maxLength(180)]],
      roles: ['', [Validators.required]]
    });
    await this.getAppUsers();
    this.appuserService.getRoles().subscribe(
      data => this.roles = data
    );
  }

  get f() { return this.frm.controls; }
  
  onSubmit() 
  { 
    this.submitted = true;
    if (this.frm.invalid) 
    {
      return;
    }

    let createForAppUserDto : CreateForAppUserDto = 
    {
      name: this.f.name.value.trim(),
      surname: this.f.surname.value.trim(),
      email: this.f.email.value.trim(),
      password: this.f.password.value.trim(),
      address: this.f.address.value.trim(),
      createDate: new Date(Date.now()),
      isActive: true,
      roleId: this.f.roles.value
    }
    if(!this.editMode)
    {
     this.appuserService.createAppUser(createForAppUserDto).subscribe(
      (res: any)=>
        {
         this.toastrService.message("Kullanıcı Kayıt Başarılı.", "Başarılı",
         {
           messageType: ToastrMessageType.Success,
           position: ToastrPosition.TopRight,
         });
           this.createdAppUser.emit(createForAppUserDto);
           this.pageChanged();
           this.frm.reset();
           this.frm.controls.roles.setValue('');
           this.submitted = false;
        },
        (error : any) => 
         {
           this.toastrService.message("Farklı Email Adresi İle Kayıt Yapınız.", "Başarısız",
           {
             messageType: ToastrMessageType.Error,
             position: ToastrPosition.TopRight,
           });
         }
      );   
    }
    else
   {
      this.appuserService.updateAppUser(this.currentAppUserId,createForAppUserDto).subscribe
     (
      (res: any)=>
        {
          this.toastrService.message("Kullanıcı Güncelleme Başarılı.", "Başarılı",
          {
            messageType: ToastrMessageType.Success,
            position: ToastrPosition.TopRight,
          });
          this.pageChanged();
          this.frm.reset();
          this.frm.controls.roles.setValue('');
          this.submitted = false;
          this.editMode = false;
        },
        (error : any) =>
        {
          this.toastrService.message("Farklı Email Adresi İle Kayıt Yapınız.", "Başarısız",
          {
            messageType: ToastrMessageType.Error,
            position: ToastrPosition.TopRight,
          });
        }
      );
    }
  }

  onEdit(id: number)
  {
    this.currentAppUserId = id;
    this.appuserService.getAppUser(id).subscribe(
      (res: any)=>
      {
        this.frm.controls.name.setValue(res.name);
        this.frm.controls.surname.setValue(res.surname);
        this.frm.controls.email.setValue(res.email);
        this.frm.controls.password.setValue(res.password);
        this.frm.controls.address.setValue(res.address);
        this.frm.controls.roles.setValue(res.roleId);
        this.editMode = true;
        this.toastrService.message(`${res.name.charAt(0).toUpperCase()}${res.name.slice(1)}'nin Bilgileri İlgili Alanlara Başarılı Gelmiştir.`, "Başarılı",
        {
          messageType: ToastrMessageType.Success,
          position: ToastrPosition.TopRight,
        });
      }
    );
  }

  onElementSelected(event,element: ListAppUserDto) 
  {
    if (event.checked) 
    {
      this.rows.push(element);
      this.toastrService.message(element.name+" "+element.surname+" seçildi.", "Uyarı",
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

  async getAppUsers() 
  {
    const allAppUsers : {totalCount : number ,listAppUsers : ListAppUserDto[]} = await this.appuserService.readAppUsers(this.paginator ? this.paginator.pageIndex : 0, this.paginator ? this.paginator.pageSize : 5);
    this.dataSource = new MatTableDataSource<ListAppUserDto>(allAppUsers.listAppUsers);
    this.paginator.length = allAppUsers.totalCount;
  }

  async pageChanged() 
  {
    await this.getAppUsers();
  }

  @Output() createdAppUser : EventEmitter<CreateForAppUserDto> = new EventEmitter();
  @ViewChild('AppusersComponent') appusersComponent : AppusersComponent;

  createdAppUserHandler() 
  {
    this.appusersComponent.getAppUsers();
  }

  getRoles() 
  {
    return this.appuserService.getRoles();
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
      XLSX.utils.book_append_sheet(workbook, worksheet, 'Kullanicilar');
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
