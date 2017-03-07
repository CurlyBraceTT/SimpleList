import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

declare var gapi: any;
declare var auth2: any;

@Component({
    selector: '[logout]',
    template: require('./logout.component.html'),
    styles: [require('./logout.component.css')]
})
export class LogoutComponent {
    constructor(private router: Router, private authService: AuthService) { }

    logout() {
        var self = this;

        auth2.signOut().then(function () {
            self.authService.logout();
            self.router.navigate(['login']);
        });
    };
}