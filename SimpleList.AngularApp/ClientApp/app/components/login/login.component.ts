import { Component, ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

declare var auth2: any;

@Component({
    selector: 'login',
    template: require('./login.component.html'),
    styles: [require('./login.component.css')]
})
export class LoginComponent {
    @ViewChild('googleButton') el: ElementRef;

    constructor(private router:Router, private authService: AuthService) { }

    ngAfterViewInit() {
        let self = this;

        if (this.authService.isLoggedIn) {
            self.router.navigate(['home']);
        }

        auth2.attachClickHandler(self.el.nativeElement, {}, (googleUser) => {
            var token = googleUser.getAuthResponse().id_token;
            var user = googleUser.getBasicProfile().getName();

            self.authService.googleLogin(token, user).then(res =>
                self.router.navigate(['home'])
            );
        });
    };
}