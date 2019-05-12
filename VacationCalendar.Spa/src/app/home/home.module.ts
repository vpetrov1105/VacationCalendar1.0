import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { CalendarService } from 'src/services/calendar.service';
import { Calendar } from 'src/models/calendar.model';
import { VacationDetailsComponent } from './vacation-details/vacation-details.component';
import {MatNativeDateModule, MAT_DATE_LOCALE} from '@angular/material';
import {DemoMaterialModule} from '../material-module';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { ConfirmationDialogComponent } from '../shared/confirmation-dialog/confirmation-dialog.component';
import { VacationDataService } from 'src/services/vacation-data.service';
import { SharedModule } from '../shared/shared.module';
import { HomeFilterComponent } from './home-filter/home-filter.component';

@NgModule({
  declarations: [
    HomeComponent,
    VacationDetailsComponent,
    HomeFilterComponent
  ],
  imports: [
    CommonModule,
    MatNativeDateModule,
    FormsModule,
    ReactiveFormsModule,
    DemoMaterialModule,
    SharedModule
  ],
  providers: [
    Calendar,
    CalendarService,
    VacationDataService,
    {provide: MAT_DATE_LOCALE, useValue: 'hr-HR'}
  ],
  entryComponents: [ConfirmationDialogComponent]
})
export class HomeModule { }
