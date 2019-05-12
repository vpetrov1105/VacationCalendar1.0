import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { map, catchError } from 'rxjs/operators';
import { Observable, throwError } from 'rxjs';
import { IUser } from 'src/interfaces/user.interface';
import { ResponseMessage } from 'src/models/response-message.model';
import { UtiliyFunctions } from 'src/infrastructure/utility-functions';


@Injectable()
export class UserService {

  constructor(private http: HttpClient) {
  }

  public getUsersData() : Observable<IUser[]> {
    
    return this.http.get<IUser[]>('/api/user/GetUsersData')
            .pipe(map(data =>{
              return data;
            } ,
            error => { console.log(error) }
            ))
  }

  insertUserData(user: IUser): Observable<ResponseMessage> {
    return this.http.post<ResponseMessage>('/api/user/InsertUserData', user)
        .pipe(map(
            data => {
                return UtiliyFunctions.handleResponse(data)
            }),
            catchError(err => {
              return throwError(UtiliyFunctions.handleError(err))
            })
           )
  }

  updateUserData(user: IUser): Observable<ResponseMessage> {
    return this.http.post<ResponseMessage>('/api/user/UpdateUserData', user)
        .pipe(map(
            data => {
                return UtiliyFunctions.handleResponse(data)
            }),
            catchError(err => {
              return throwError(UtiliyFunctions.handleError(err))
            })
           )
  }

  deleteUserData(user: IUser): Observable<ResponseMessage> {


    return this.http.post<ResponseMessage>('/api/user/DeleteUserData', user.id)
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
