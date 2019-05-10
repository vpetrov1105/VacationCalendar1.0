import { Injectable } from '@angular/core';
import { VacationDataService } from 'src/services/vacation-data.service';
import { ResponseMessage } from './response-message.model';
import { Observable, throwError } from 'rxjs';
import { map, catchError } from 'rxjs/operators';
import { IVacationData } from 'src/interfaces/vacation-data.interface';

@Injectable()
export class VacationData {
    id: number

    constructor(private vacationService: VacationDataService) { 
    }

    public deleteVacation(vacation: IVacationData) : Observable<ResponseMessage>{
        return this.vacationService.deleteVacation(vacation)
        .pipe(map(data => {
            return data;
        }),
        catchError(err => {
            return throwError(err)
        })
        );
    }

    public updateVacation(vacation: IVacationData[]) : Observable<ResponseMessage>{
        return this.vacationService.updateVacation(vacation)
        .pipe(map(data => {
            return data;
        }),
        catchError(err => {
            return throwError(err)
        })
        );
    }
}