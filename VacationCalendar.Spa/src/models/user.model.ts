import { IVacationData } from 'src/interfaces/vacation-data.interface';
import { IUser } from 'src/interfaces/user.interface';

export class User {

    id: number
    firstName: string 
    lastName: string
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
            user.vacationData = iUser.vacationData

            users.push(user)
        }
        
        return users
    }

}