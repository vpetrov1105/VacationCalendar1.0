import { Injectable, Injector } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';
import { tap } from 'rxjs/operators';
import { Observable } from 'rxjs';
import { AuthenticationService } from 'src/services/authentication.service';


@Injectable()
export class SpinnerInterceptor implements HttpInterceptor {
    constructor(private spinnerService: Ng4LoadingSpinnerService) { }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        return next.handle(req).pipe(tap(event => {
            this.spinnerService.show()
            if (event instanceof HttpResponse) {
                this.spinnerService.hide()
                return event
            }
        }, (error: HttpErrorResponse) => {
            this.spinnerService.hide()
        }))
    }
}