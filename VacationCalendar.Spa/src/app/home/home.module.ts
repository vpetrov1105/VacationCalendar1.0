import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from './home.component';
import { CalendarService } from 'src/services/calendar.service';
import { Calendar } from 'src/models/calendar.model';
import { StaticListsService } from 'src/services/static-lists.service';
import { VacationDetailsComponent } from './vacation-details/vacation-details.component';
import {MatNativeDateModule} from '@angular/material';
import {DemoMaterialModule} from '../material-module';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';
import { VacationDataService } from 'src/services/vacation-data.service';
import { VacationData } from 'src/models/vacation-data.model';
import { ValidationSummaryComponent } from './validation-summary/validation-summary.component';

@NgModule({
  declarations: [
    HomeComponent,
    VacationDetailsComponent,
    ConfirmationDialogComponent,
    ValidationSummaryComponent
  ],
  imports: [
    CommonModule,
    MatNativeDateModule,
    FormsModule,
    ReactiveFormsModule,
    DemoMaterialModule
  ],
  providers: [
    Calendar,
    CalendarService,
    StaticListsService,
    VacationDataService,
    VacationData
  ],
  entryComponents: [ConfirmationDialogComponent]
})
export class HomeModule { }
