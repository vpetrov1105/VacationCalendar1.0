import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { IStaticLists } from 'src/interfaces/static-lists.interface';
import * as moment from 'moment';
import {Moment} from 'moment';
import { MatDatepicker, DateAdapter, MAT_DATE_LOCALE, MAT_DATE_FORMATS } from '@angular/material';
import { HomeFilter } from 'src/models/home-filter.model';
import {MomentDateAdapter} from '@angular/material-moment-adapter'

export const MY_FORMATS = {
  parse: {
    dateInput: 'MM/YYYY',
  },
  display: {
    dateInput: 'MM/YYYY',
    monthYearLabel: 'MMM YYYY',
    dateA11yLabel: 'LL',
    monthYearA11yLabel: 'MMMM YYYY',
  },
}

@Component({
  selector: 'app-home-filter',
  templateUrl: './home-filter.component.html',
  styleUrls: ['./home-filter.component.css'],
  providers: [
    {provide: DateAdapter, useClass: MomentDateAdapter, deps: [MAT_DATE_LOCALE]},
    {provide: MAT_DATE_FORMATS, useValue: MY_FORMATS}
  ]
})

export class HomeFilterComponent implements OnInit {
  @Input() staticLists: IStaticLists
  @Output() filterValues: EventEmitter<HomeFilter>
  
  homeFilterDataGroup: FormGroup
  firstNameCntrl: FormControl
  lastNameCntrl: FormControl
  monthYearCntrl: FormControl
  vacationTypeCntrl: FormControl
  
  constructor() { 
    this.filterValues = new EventEmitter<HomeFilter>()
  }

  ngOnInit() {
    this.createFormControls()
    this.createForm()
  }

  createFormControls() {
    this.firstNameCntrl = new FormControl('')
    this.lastNameCntrl = new FormControl('')
    this.monthYearCntrl = new FormControl(moment(), Validators.required)
    this.vacationTypeCntrl = new FormControl('')

  }

  createForm() {
    this.homeFilterDataGroup = new FormGroup({
      firstNameCntrl: this.firstNameCntrl,
      lastNameCntrl: this.lastNameCntrl,
      monthYearCntrl: this.monthYearCntrl,
      vacationTypeCntrl: this.vacationTypeCntrl
    })
  }

  chosenYearHandler(normalizedYear: Moment) {
    const ctrlValue = this.monthYearCntrl.value;
    ctrlValue.year(normalizedYear.year());
    this.monthYearCntrl.setValue(ctrlValue);
  }

  chosenMonthHandler(normalizedMonth: Moment, datepicker: MatDatepicker<Moment>) {
    const ctrlValue = this.monthYearCntrl.value;
    ctrlValue.month(normalizedMonth.month());
    this.monthYearCntrl.setValue(ctrlValue);
    datepicker.close();
  }

  setFilter(){
    this.filterValues.emit(this.setFilterValues(new Date(this.monthYearCntrl.value)))
  }

  setFilterValues(date: Date) : HomeFilter{
    let filterValues = new HomeFilter()
    filterValues.firstName = this.firstNameCntrl.value
    filterValues.lastName = this.lastNameCntrl.value
    filterValues.vacationType = this.vacationTypeCntrl.value
    filterValues.month = date.getMonth() + 1
    filterValues.year = date.getFullYear()
    return filterValues
  }

  setMonthYearValue(date: Date){
    this.monthYearCntrl.setValue(moment(date))
  }

  clearFilter(){
    this.firstNameCntrl.setValue("")
    this.lastNameCntrl.setValue("")
    this.vacationTypeCntrl.setValue("")
    this.monthYearCntrl.setValue(moment(new Date()))
  }

  resetFilter(){
    this.clearFilter()
    this.filterValues.emit(this.setFilterValues(new Date()))
  }

}
