import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { map, catchError } from 'rxjs/operators';
import { ResponseMessage } from 'src/models/response-message.model';
import { Observable, throwError } from 'rxjs';
import { UtiliyFunctions } from 'src/infrastructure/utility-functions';
import { IVacationData } from 'src/interfaces/vacation-data.interface';

@Injectable({
  providedIn: 'root'
})
export class VacationDataService {

  constructor(private http: HttpClient) { }

  deleteVacation(vacation: IVacationData[]): Observable<ResponseMessage> {
    return this.http.post<ResponseMessage>('/api/calendar/DeleteVacation', vacation)
        .pipe(map(
            data => {
              return UtiliyFunctions.handleResponse(data)
            }),
            catchError(err => {
              return throwError(UtiliyFunctions.handleError(err))
            })
           )
  }

  updateVacation(vacation: IVacationData[]): Observable<ResponseMessage> {
    return this.http.post<ResponseMessage>('/api/calendar/updateVacation', vacation)
        .pipe(map(
            data => {
                return UtiliyFunctions.handleResponse(data)
            }),
            catchError(err => {
              return throwError(UtiliyFunctions.handleError(err))
            })
           )
  }
}
