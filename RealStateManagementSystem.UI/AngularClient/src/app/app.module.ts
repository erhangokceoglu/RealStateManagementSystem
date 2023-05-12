import { BrowserModule } from '@angular/platform-browser';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SistemyoneticisiModule } from './sistemyoneticisi/sistemyoneticisi.module';
import { UiModule } from './ui/ui.module';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { ToastrModule } from 'ngx-toastr';
import { HTTP_INTERCEPTORS, HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { DeletedialogComponent } from './dialogs/deletedialog/deletedialog.component';
import { JwtModule } from '@auth0/angular-jwt';
import { HttpErrorInterceptorService } from './services/common/http-error-interceptor.service';
import { NgxSelectModule } from 'ngx-select-ex';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    SistemyoneticisiModule,
    UiModule,
    ToastrModule.forRoot(),
    HttpClientModule,
    FormsModule,
    NgxSelectModule,
    JwtModule.forRoot({
      config: {
        tokenGetter: function tokenGetter() {
            return localStorage.getItem('token');
          },
        whitelistedDomains: ['localhost:7219']
      }
    }),
  ],
  providers: [
    { provide: 'baseUrl', useValue: 'https://localhost:7219/api', multi : true},
    { provide: HTTP_INTERCEPTORS, useClass: HttpErrorInterceptorService, multi: true}
  ],
  entryComponents: [DeletedialogComponent],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA]
})
export class AppModule { }
