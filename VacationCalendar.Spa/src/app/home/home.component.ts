import { Component, OnInit, ViewChild } from '@angular/core';
import { IVacationData } from '../../interfaces/vacation-data.interface';
import { Calendar } from 'src/models/calendar.model';
import { IStaticLists } from '../../interfaces/static-lists.interface';
import { MatDialogRef, MatDialog } from '@angular/material';
import { ConfirmationDialogComponent } from '../shared/confirmation-dialog/confirmation-dialog.component';
import { ResponseMessage } from 'src/models/response-message.model';
import { User } from 'src/models/user.model';
import { AuthenticationService } from 'src/services/authentication.service';
import { ILoginUser } from 'src/interfaces/login-user.interface';
import { Role } from 'src/models/role.model';
import { VacationDataService } from 'src/services/vacation-data.service';
import { HomeFilter } from 'src/models/home-filter.model';
import { HomeFilterComponent } from './home-filter/home-filter.component';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  @ViewChild(HomeFilterComponent) filterComp:HomeFilterComponent;
  currentUser: ILoginUser
  staticLists: IStaticLists
  dayNames : IVacationData[]
  dialogRef: MatDialogRef<ConfirmationDialogComponent>
  response: ResponseMessage
  get role() { return Role; }

  constructor(private calendarData: Calendar, public dialog: MatDialog, private vacationService: VacationDataService,
    private authenticationService: AuthenticationService) { 
      this.currentUser = this.authenticationService.currentUserValue
  }

  ngOnInit() {
    this.calendarData.getInitialData().subscribe(
      data => {
          this.staticLists = data.staticLists
          this.calendarData = data.calendar
          this.dayNames =  this.calendarData.users[0].vacationData
        },
            error => console.log(error)
    )
  }

  ngOnDestroy() {
    this.calendarData.isInEditMode=false
  }

  changeMonth(action: string) {
    this.response = null
    this.calendarData.getCalendarData(action).subscribe( data => {
      this.calendarData = data,
      this.dayNames =  this.calendarData.users[0].vacationData
      this.filterComp.setMonthYearValue(new Date(this.calendarData.year, this.calendarData.month -1, 1,0,0,0,0))
    },
        error => console.log(error)
    )
  }

  changeMode(action: boolean){
    this.response = null
    this.calendarData.isInEditMode = action;
  }

  insertVacation(user: User){
    this.response = null
    if(user.isDetailShown){
      user.isDetailShown = false
      return
    }
    user.isDetailShown = true
  }

  deleteVacation(vacation: IVacationData, userIndex: number){
    this.response = null
    this.dialogRef = this.dialog.open(ConfirmationDialogComponent)
        this.dialogRef.componentInstance.confirmMessage = "Are you sure you want to delete vacation day?"
        this.dialogRef.updatePosition({ top: '3%' })
        this.dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.vacationService.deleteVacation(vacation)
                    .subscribe(
                        data => {
                            if (data.success) {
                              if(data.returnedObject != undefined){
                                this.calendarData.users[userIndex].vacationData = data.returnedObject
                              }
                            }
                            this.response = data
                        },
                        error => {
                          if(error.returnedObject != undefined){
                            this.calendarData.users[userIndex].vacationData = error.returnedObject
                          }
                            this.response = error
                        }
                    )
            }
            this.dialogRef = null
        })
  }

  getVacationTypeName(typeID: number) : string{
    let type = this.staticLists.vacationTypes.find(x => x.id === typeID)
    if (type != undefined) {
        return type.vacationTypeName
    }
    return ""
  }

  responseVacation(event: ResponseMessage) {
    this.response = event

  }

  filterValues(event: HomeFilter) {
      this.response = null
      this.calendarData.getFilteredCalendar(event).subscribe( data => {
      this.calendarData = data,
      this.dayNames =  this.calendarData.users[0].vacationData
    },
        error => console.log(error)
    )
  }



}
