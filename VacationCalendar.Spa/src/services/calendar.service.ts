import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { map } from 'rxjs/operators';
import { ISelectedPeriod } from 'src/interfaces/selected-period.interface';
import { Observable } from 'rxjs';
import { ICalendar } from 'src/interfaces/calendar.interface';
import { IInitialData} from 'src/interfaces/initial-data.interface';
import { HomeFilter } from 'src/models/home-filter.model';


@Injectable()
export class CalendarService {

  constructor(private http: HttpClient) {
  }

  public getInitialData(period?: ISelectedPeriod) : Observable<IInitialData> {
    
    let params = new HttpParams().set('year', String(period.year)).set('month', String(period.month))

    return this.http.get<IInitialData>('/api/calendar/GetInitialData', { params: params })
            .pipe(map(data =>{
              return data;
            } ,
            error => { console.log(error) }
            ))
  }

  public getCalendarData(period?: ISelectedPeriod) : Observable<ICalendar> {
    
    let params = new HttpParams().set('year', String(period.year)).set('month', String(period.month))

    return this.http.get<ICalendar>('/api/calendar/GetCalendarData', { params: params })
            .pipe(map(data =>{
              return data;
            } ,
            error => { console.log(error) }
            ))
  }

  public getFilteredCalendar(filter: HomeFilter) : Observable<ICalendar> {

    let params = new HttpParams().set('firstName', String(filter.firstName)).set('lastName', String(filter.lastName))
    .set('vacationType', String(filter.vacationType)).set('year', String(filter.year)).set('month', String(filter.month))
  
    return this.http.get<ICalendar>('/api/calendar/GetFilteredCalendar', { params: params })
            .pipe(map(data =>{
              return data;
            } ,
            error => { console.log(error) }
            ))
  }

}
