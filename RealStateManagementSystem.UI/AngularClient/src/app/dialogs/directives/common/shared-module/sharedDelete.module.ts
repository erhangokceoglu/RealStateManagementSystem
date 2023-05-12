import { NgModule } from '@angular/core';
import { DeleteDirective } from 'src/app/directives/common/delete.directive';

@NgModule({
  declarations: [DeleteDirective],
  exports: [DeleteDirective]
})
export class SharedDeleteModule { }
