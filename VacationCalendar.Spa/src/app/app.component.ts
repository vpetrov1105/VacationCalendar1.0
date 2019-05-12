import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthenticationService } from 'src/services/authentication.service';
import { ILoginUser } from 'src/interfaces/login-user.interface';
import { Role } from 'src/models/role.model';
import { Ng4LoadingSpinnerService } from 'ng4-loading-spinner';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})

export class AppComponent {
    title = 'VacationCalendar';
    currentUser: ILoginUser;

    constructor(
        private router: Router,
        private authenticationService: AuthenticationService, private spinnerService: Ng4LoadingSpinnerService
    ) {
        this.authenticationService.currentUser.subscribe(x => this.currentUser = x);
        spinnerService.show();
    }

    logout() {
        this.authenticationService.logout();
        this.router.navigate(['/login']);
    }
}

