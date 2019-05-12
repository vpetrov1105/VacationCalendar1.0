import { IVacationData } from './vacation-data.interface';

export interface IUser {

    id: number
    firstName: string 
    lastName: string
    birthDate: Date
    userName: string
    role: string
    officeCountryCode: string
    rowVersion: any
    vacationData: IVacationData[]
}
