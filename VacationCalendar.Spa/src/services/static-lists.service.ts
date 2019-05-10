import { Injectable } from '@angular/core';
import { HttpClient, HttpParams, HttpHeaders } from "@angular/common/http";
import { map } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { IStaticLists } from 'src/interfaces/static-lists.interface';


@Injectable()
export class StaticListsService {

  constructor(private http: HttpClient) {
  }

  public getStaticLists() : Observable<IStaticLists> {

    return this.http.get<IStaticLists>('/api/staticlists/GetStaticLists')
            .pipe(map(data =>{
              return data;
            } ,
            error => { console.log(error) }
            ))
  }

}
