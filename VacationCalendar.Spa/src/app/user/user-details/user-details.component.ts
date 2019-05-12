import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { User } from 'src/models/user.model';
import { IStaticLists } from 'src/interfaces/static-lists.interface';
import { ResponseMessage } from 'src/models/response-message.model';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { UserService } from 'src/services/user.service';
import { IUser } from 'src/interfaces/user.interface';
import { ILoginUser } from 'src/interfaces/login-user.interface';
import { Role } from 'src/models/role.model';

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css']
})
export class UserDetailsComponent implements OnInit {
  @Input() user: User
  @Input() staticLists: IStaticLists
  @Input() currentUser: ILoginUser
  @Output() responseUser: EventEmitter<ResponseMessage>
  get role() { return Role; }

  @Input() isNew: boolean

  userDataGroup: FormGroup
  firstNameCntrl: FormControl
  lastNameCntrl: FormControl
  dateBirthCntrl: FormControl
  userNameCntrl: FormControl
  roleCntrl: FormControl
  officeCodeCntrl: FormControl
  
  constructor(private userService: UserService) {
    this.responseUser = new EventEmitter<ResponseMessage>()
  }

  ngOnInit() {
    this.createFormControls()
    this.createForm()

    if(this.currentUser.role != Role.admin){
      this.userDataGroup.get('roleCntrl').disable();
    }
  }

  createFormControls() {
    this.firstNameCntrl = new FormControl(this.user.firstName, Validators.required)
    this.lastNameCntrl = new FormControl(this.user.lastName, Validators.required)
    this.dateBirthCntrl = new FormControl(this.user.birthDate, Validators.required)
    this.userNameCntrl = new FormControl(this.user.userName, Validators.required)
    this.roleCntrl = new FormControl(this.user.role, Validators.required)
    this.officeCodeCntrl = new FormControl(this.user.officeCountryCode, Validators.required)

  }

  createForm() {
    this.userDataGroup = new FormGroup({
      firstNameCntrl: this.firstNameCntrl,
      lastNameCntrl: this.lastNameCntrl,
      dateBirthCntrl: this.dateBirthCntrl,
      userNameCntrl: this.userNameCntrl,
      roleCntrl: this.roleCntrl,
      officeCodeCntrl: this.officeCodeCntrl
    })
  }

  saveData(){
    if (this.userDataGroup.valid) 
    {
      let user = {} as IUser
      user.id = this.user.id
      user.rowVersion = this.user.rowVersion
      user.firstName = this.firstNameCntrl.value
      user.lastName = this.lastNameCntrl.value
      user.birthDate = this.dateBirthCntrl.value
      user.userName = this.userNameCntrl.value
      user.role = this.roleCntrl.value
      user.officeCountryCode = this.officeCodeCntrl.value

      if(this.isNew){
        this.userService.insertUserData(user)
        .subscribe(
            data => {
                if (data.success) {
                  this.responseUser.emit(data) 
                  this.userDataGroup.reset();
              }
            },
            error => {
              this.responseUser.emit(error) 
            }
        )
      }
      else{
        this.userService.updateUserData(user)
        .subscribe(
            data => {
                if (data.success) {
                  this.responseUser.emit(data) 
              }
            },
            error => {
              this.responseUser.emit(error) 
            }
        )
      }
      

      
    }

  }
}
