import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { map } from 'rxjs/operators';
import { ISelectedPeriod } from 'src/interfaces/selected-period.interface';
import { Observable } from 'rxjs';
import { ICalendar } from 'src/interfaces/calendar.interface';


@Injectable()
export class CalendarService {

  constructor(private http: HttpClient) {
  }

  public getCalendarData(period?: ISelectedPeriod) : Observable<ICalendar> {
    
    let params = new HttpParams().set('year', String(period.year)).set('month', String(period.month))

    return this.http.get<ICalendar>('/api/calendar/GetCalendar', { params: params })
            .pipe(map(data =>{
              return data;
            } ,
            error => { console.log(error) }
            ))
  }

}
