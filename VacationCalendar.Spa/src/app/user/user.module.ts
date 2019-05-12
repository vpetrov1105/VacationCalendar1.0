import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { UserComponent } from './user.component';
import { MatNativeDateModule, MatInputModule, MAT_DATE_LOCALE } from '@angular/material';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { DemoMaterialModule } from '../material-module';
import { UserService } from 'src/services/user.service';
import { ConfirmationDialogComponent } from '../shared/confirmation-dialog/confirmation-dialog.component';
import { UserDetailsComponent } from './user-details/user-details.component';
import { SharedModule } from '../shared/shared.module';

@NgModule({
  declarations: [
    UserComponent,
    UserDetailsComponent
  ],
  imports: [
    CommonModule,
    MatNativeDateModule,
    FormsModule,
    ReactiveFormsModule,
    DemoMaterialModule,
    MatInputModule,
    SharedModule
  ],
  providers: [
    UserService,
    {provide: MAT_DATE_LOCALE, useValue: 'hr-HR'}
  ],
  entryComponents: [ConfirmationDialogComponent]
})
export class UserModule { }
