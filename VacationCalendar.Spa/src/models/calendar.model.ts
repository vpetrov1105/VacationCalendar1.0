import { IUser } from 'src/interfaces/user.interface';
import { CalendarService } from 'src/services/calendar.service';
import { ISelectedPeriod } from 'src/interfaces/selected-period.interface';
import { UtiliyFunctions } from 'src/infrastructure/utility-functions';
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { ICalendar } from 'src/interfaces/calendar.interface';
import { Injectable } from '@angular/core';
import { User } from './user.model';
import { IInitialData } from 'src/interfaces/initial-data.interface';
import { InitialData } from './initial-data.model';
import { HomeFilter } from './home-filter.model';

@Injectable()
export class Calendar {

    year: number
    month: number 
    monthName: string
    users: IUser[]
    isInEditMode: boolean = false

    constructor(private calendarService: CalendarService) { 
    }

    public getInitialData(period?: ISelectedPeriod) : Observable<InitialData>{

        if(period === undefined || period === null) {
            period = UtiliyFunctions.currentMonthYear;
        }
        
         return this.calendarService.getInitialData(period)
            .pipe(map(data => {
                let initialData = new InitialData()
                initialData.staticLists = data.staticLists
                initialData.calendar = this.parseServerData(data.calendar)
                return initialData;
            }));
    }

    public getCalendarData(direction: string): Observable<Calendar>  {
        let period = this.generatePeriod(direction);
        return this.calendarService.getCalendarData(period)
        .pipe(map(data => {
            let parsedData = this.parseServerData(data)
            return parsedData
        }));
    }

    public getFilteredCalendar(filter: HomeFilter): Observable<Calendar>  {
        return this.calendarService.getFilteredCalendar(filter)
            .pipe(map(data => {
                let parsedData = this.parseServerData(data)
                return parsedData
            }));
    }

    private parseServerData(data: ICalendar): Calendar {

        this.year = data.year
        this.month = data.month
        this.monthName = data.monthName
        let user = new User()
        this.users = user.parseUserData(data.users)
        
        return this
    }  

    private generatePeriod(direction: string) : ISelectedPeriod {
        var period = <ISelectedPeriod>{};

        if(this.month !== undefined && this.month !== null) {
            period.month = this.month
            period.year = this.year
        }
        else 
            period = UtiliyFunctions.currentMonthYear;

        switch (direction.toLowerCase()) {
            case 'nextmonth':
                period.month++;
                break;
            case 'previousmonth':
                period.month--;
                break;
        }

        if (period.month > 12) {
            period.month = 1
            period.year++;
        }

        if (period.month < 1) {
            period.month = 12;
            period.year--;
        }

        return period
    }

    
}
