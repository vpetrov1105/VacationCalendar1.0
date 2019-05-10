import { IVacationData } from './vacation-data.interface';

export interface IUser {

    id: number
    firstName: string 
    lastName: string
    vacationData: IVacationData[]
}
