import { IUser } from './user.interface';

export interface ICalendar {

    year: number
    month: number 
    monthName: string
    users: IUser[]
}
