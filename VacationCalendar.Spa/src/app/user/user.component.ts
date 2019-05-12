import { Component, OnInit, ViewChild } from '@angular/core';
import { UserService } from 'src/services/user.service';
import { User } from 'src/models/user.model';
import { ResponseMessage } from 'src/models/response-message.model';
import { AuthenticationService } from 'src/services/authentication.service';
import { ILoginUser } from 'src/interfaces/login-user.interface';
import { Role } from 'src/models/role.model';
import { ConfirmationDialogComponent } from '../shared/confirmation-dialog/confirmation-dialog.component';
import { MatDialogRef, MatDialog } from '@angular/material';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  currentUser: ILoginUser
  users: User[]
  response: ResponseMessage
  dialogRef: MatDialogRef<ConfirmationDialogComponent>
  newUser: User
  get role() { return Role; }

  constructor(private userService: UserService, private authenticationService: AuthenticationService,  public dialog: MatDialog) { 
    this.currentUser = this.authenticationService.currentUserValue
  }

  ngOnInit() {
    this.userService.getUsersData().subscribe(
      data => {
          let user = new User()
          this.users = user.parseUserData(data)
        },
            error => console.log(error)
    )
  }


  editUser(user: User){
    this.response = null
    if(user.isDetailShown){
      user.isDetailShown = false
      return
    }
    user.isDetailShown = true
  }

  deleteUser(user: User){
    this.response = null
    this.dialogRef = this.dialog.open(ConfirmationDialogComponent)
        this.dialogRef.componentInstance.confirmMessage = "Are you sure you want to delete user?"
        this.dialogRef.updatePosition({ top: '3%' })
        this.dialogRef.afterClosed().subscribe(result => {
            if (result) {
                this.userService.deleteUserData(user)
                    .subscribe(
                        data => {
                            if (data.success) {
                                let i = this.users.findIndex(i => i.id === user.id)
                                this.users.splice(i ,1)
                            }
                            this.response = data
                        },
                        error => {
                            this.response = error
                        }
                    )
            }
            this.dialogRef = null
        })
  }

  insertUser(){
    this.response = null
    if(this.newUser != undefined && this.newUser.isDetailShown){
      this.newUser.isDetailShown = false
      return
    }
    this.newUser = new User();
    this.newUser.isDetailShown = true

  }

  responseUser(event: ResponseMessage) {
    if(event.returnedObject != undefined){
      let temp = event.returnedObject as User
      let i = this.users.findIndex(i => i.id === temp.id)
      let changedUser = new User()
      changedUser.id = temp.id
      changedUser.firstName = temp.firstName
      changedUser.lastName = temp.lastName
      changedUser.birthDate = temp.birthDate
      changedUser.userName = temp.userName
      changedUser.role = temp.role
      changedUser.officeCountryCode = temp.officeCountryCode
      changedUser.rowVersion = temp.rowVersion
      if(i < 0){
        this.users.push(changedUser)
        if(event.success){
          this.newUser = undefined
        }
      }
      else{
        this.users[i] = changedUser
        if(event.success){
          this.users[i].isDetailShown = false
        }
      }
     
      
    }

    this.response = event
  }


}
