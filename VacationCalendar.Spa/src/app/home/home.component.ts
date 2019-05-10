import { Component, OnInit } from '@angular/core';
import { IVacationData } from '../../interfaces/vacation-data.interface';
import { Calendar } from 'src/models/calendar.model';
import { IStaticLists } from '../../interfaces/static-lists.interface';
import { StaticListsService } from 'src/services/static-lists.service';
import { MatDialogRef, MatDialog } from '@angular/material';
import { ConfirmationDialogComponent } from './confirmation-dialog/confirmation-dialog.component';
import { VacationData } from 'src/models/vacation-data.model';
import { ResponseMessage } from 'src/models/response-message.model';
import { User } from 'src/models/user.model';
import { AuthenticationService } from 'src/services/authentication.service';
import { ILoginUser } from 'src/interfaces/login-user.interface';
import { Role } from 'src/models/role.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  currentUser: ILoginUser;
  staticLists: IStaticLists
  dayNames : IVacationData[]
  dialogRef: MatDialogRef<ConfirmationDialogComponent>
  response: ResponseMessage
  get role() { return Role; }

  constructor(private staticListsService: StaticListsService, private currentMonthData: Calendar, public dialog: MatDialog, private vacationData: VacationData,
    private authenticationService: AuthenticationService) { 
      this.currentUser = this.authenticationService.currentUserValue
  }

  ngOnInit() {
    this.staticListsService.getStaticLists().subscribe(data => {
      this.staticLists = data
    },
      error => console.log(error))

    this.currentMonthData.loadCalendarData().subscribe(
      data => {this.currentMonthData = data,
              this.dayNames =  this.currentMonthData.users[0].vacationData
        },
            error => console.log(error)
    )
  }

  ngOnDestroy() {
    this.currentMonthData.isInEditMode=false
  }

  changeMonth(action: string) {
    this.response = null
    this.currentMonthData.changeMonth(action).subscribe( data => {
      this.currentMonthData = data,
      this.dayNames =  this.currentMonthData.users[0].vacationData
    },
        error => console.log(error)
    )
  }

  changeMode(action: boolean){
    this.response = null
    this.currentMonthData.isInEditMode = action;
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
                this.vacationData.deleteVacation(vacation)
                    .subscribe(
                        data => {
                            if (data.success) {
                              if(data.returnedObject != undefined){
                                this.currentMonthData.users[userIndex].vacationData = data.returnedObject
                              }
                            }
                            this.response = data
                        },
                        error => {
                          if(error.returnedObject != undefined){
                            this.currentMonthData.users[userIndex].vacationData = error.returnedObject
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



}
