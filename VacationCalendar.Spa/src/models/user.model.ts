import { IVacationData } from 'src/interfaces/vacation-data.interface';
import { IUser } from 'src/interfaces/user.interface';

export class User {

    id: number = 0
    firstName: string = ""
    lastName: string = ""
    birthDate: Date = null
    userName: string = ""
    role: string = ""
    officeCountryCode: string = ""
    rowVersion: any
    vacationData: IVacationData[]
    isDetailShown: boolean = false

    constructor() { 
    }

    public parseUserData(iUserData: IUser[]) : User[] {
        
        let users: User[] = []
        for(let iUser of iUserData){
            let user = new User()
            user.id = iUser.id
            user.firstName = iUser.firstName
            user.lastName = iUser.lastName
            user.birthDate = iUser.birthDate
            user.userName = iUser.userName
            user.role = iUser.role
            user.officeCountryCode = iUser.officeCountryCode
            user.rowVersion = iUser.rowVersion
            user.vacationData = iUser.vacationData

            users.push(user)
        }
        
        return users
    }

}