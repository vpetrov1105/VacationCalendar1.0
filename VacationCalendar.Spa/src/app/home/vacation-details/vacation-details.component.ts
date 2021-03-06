import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { IStaticLists } from 'src/interfaces/static-lists.interface';
import { User } from 'src/models/user.model';
import { MatDatepickerInputEvent, MatDialogRef, MatDialog } from '@angular/material';
import { IVacationData } from 'src/interfaces/vacation-data.interface';
import { ResponseMessage } from 'src/models/response-message.model';
import { VacationDataService } from 'src/services/vacation-data.service';
import { ConfirmationDialogComponent } from 'src/app/shared/confirmation-dialog/confirmation-dialog.component';

@Component({
  selector: 'app-vacation-details',
  templateUrl: './vacation-details.component.html',
  styleUrls: ['./vacation-details.component.css']
})

export class VacationDetailsComponent implements OnInit {
  @Input() user: User
  @Input() staticLists: IStaticLists
  @Output() responseVacation: EventEmitter<ResponseMessage>


  minDateFrom: Date
  maxDateFrom: Date
  minDateTo: Date
  maxDateTo: Date

  vacationDataGroup: FormGroup
  dateFromCntrl: FormControl
  dateToCntrl: FormControl
  vacationTypeCntrl: FormControl

  dialogRef: MatDialogRef<ConfirmationDialogComponent>
  
  constructor(private vacationService: VacationDataService, public dialog: MatDialog) {
    this.createFormControls()
    this.createForm()
    this.responseVacation = new EventEmitter<ResponseMessage>()

   }

  ngOnInit() {
    this.minDateFrom = this.user.vacationData[0].calendarDate
    this.maxDateFrom = this.user.vacationData[this.user.vacationData.length-1].calendarDate
    this.minDateTo = this.user.vacationData[0].calendarDate
    this.maxDateTo = this.user.vacationData[this.user.vacationData.length-1].calendarDate
  }

  createFormControls() {
    this.dateFromCntrl = new FormControl('', Validators.required)
    this.dateToCntrl = new FormControl('', Validators.required)
    this.vacationTypeCntrl = new FormControl('', Validators.required)

}

createForm() {
    this.vacationDataGroup = new FormGroup({
      dateFromCntrl: this.dateFromCntrl,
      dateToCntrl: this.dateToCntrl,
      vacationTypeCntrl: this.vacationTypeCntrl
    })
}

saveData(){
  if (this.vacationDataGroup.valid) 
  {
    let vacationTemp: IVacationData[] = []
    for(let vacation of this.user.vacationData){
      if(new Date(vacation.calendarDate) >= new Date(this.dateFromCntrl.value) && new Date(vacation.calendarDate) <= new Date(this.dateToCntrl.value))
      {
        let updatedData: IVacationData
        updatedData = Object.assign({}, vacation);
        updatedData.isOnVacation = true
        updatedData.vacationTypeID = this.vacationTypeCntrl.value
        updatedData.rowVersion = vacation.rowVersion
        vacationTemp.push(updatedData)
      }
    }

    this.vacationService.updateVacation(vacationTemp)
    .subscribe(
        data => {
            if (data.success) {
              if(data.returnedObject != undefined){
                this.updateContent(data)
              }
              this.responseVacation.emit(data) 
            }
        },
        error => {
          if(error.returnedObject != undefined){
            this.updateContent(error)
          }
          this.responseVacation.emit(error) 
        }
    )
  }

}

deleteData(){
  if (this.vacationDataGroup.valid) 
  {
    this.dialogRef = this.dialog.open(ConfirmationDialogComponent)
        this.dialogRef.componentInstance.confirmMessage = "Are you sure you want to delete vacation for choosen period and type?"
        this.dialogRef.updatePosition({ top: '3%' })
        this.dialogRef.afterClosed().subscribe(result => {
            if (result) {
              let vacationTemp: IVacationData[] = []
              for(let vacation of this.user.vacationData){
                if(new Date(vacation.calendarDate) >= new Date(this.dateFromCntrl.value) && new Date(vacation.calendarDate) <= new Date(this.dateToCntrl.value) &&
                  vacation.vacationTypeID == Number(this.vacationTypeCntrl.value))
                {
                  vacationTemp.push(vacation)
                }
              }
              this.vacationService.deleteVacation(vacationTemp)
                  .subscribe(
                      data => {
                          if (data.success) {
                            if(data.returnedObject != undefined){
                              this.updateContent(data)
                            }
                            this.responseVacation.emit(data) 
                          }
                      },
                      error => {
                        if(error.returnedObject != undefined){
                          this.updateContent(error)
                        }
                        this.responseVacation.emit(error) 
                      }
                  )
            }
            this.dialogRef = null
        })
  }
}


fromEvent(type: string, event: MatDatepickerInputEvent<Date>) {
  this.minDateTo = event.value
}

toEvent(type: string, event: MatDatepickerInputEvent<Date>) {
  this.maxDateFrom = event.value
}

private updateContent(response: ResponseMessage){
  if(response.success){
    this.user.isDetailShown = false
  }
  this.user.vacationData = response.returnedObject as IVacationData[];
}

}



