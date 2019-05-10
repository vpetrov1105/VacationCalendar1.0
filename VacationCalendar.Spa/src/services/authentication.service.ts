import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { ILoginUser } from 'src/interfaces/login-user.interface';


@Injectable({ providedIn: 'root' })
export class AuthenticationService {
    private currentUserSubject: BehaviorSubject<ILoginUser>;
    public currentUser: Observable<ILoginUser>;

    constructor(private http: HttpClient) {
        this.currentUserSubject = new BehaviorSubject<ILoginUser>(JSON.parse(localStorage.getItem('currentUser')));
        this.currentUser = this.currentUserSubject.asObservable();
    }

    public get currentUserValue(): ILoginUser {
        return this.currentUserSubject.value;
    }

    login(username: string, password: string) {
        return this.http.post<any>('/api/Authentication/authenticate', { username, password })
            .pipe(map(user => {
                // login successful if there's a jwt token in the response
                if (user && user.token) {
                    // store user details and jwt token in local storage to keep user logged in between page refreshes
                    localStorage.setItem('currentUser', JSON.stringify(user));
                    this.currentUserSubject.next(user);
                }

                return user;
            }));
    }

    logout() {
        // remove user from local storage to log user out
        localStorage.removeItem('currentUser');
        this.currentUserSubject.next(null);
    }

    getAllUsers() {
        return this.http.get<ILoginUser[]>('/api/authentication/GetUsers');
    }

    getUserById(id: number) {
        let params = new HttpParams().set('id', String(id))
        return this.http.get<ILoginUser>('/api/authentication/GetUser', { params: params })
    }
}