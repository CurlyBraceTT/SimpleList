import { Component, NgZone } from '@angular/core';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'nav-menu',
    template: require('./navmenu.component.html'),
    styles: [require('./navmenu.component.css')]
})
export class NavMenuComponent {
    isLogged: boolean = false;
    user: string = '';

    constructor(private zone:NgZone, private authService: AuthService) {
        var self = this;
        self.isLogged = this.authService.isLoggedIn;
        self.user = this.authService.getUser();

        authService.loggedSubject$.subscribe(
            isLogged => {
                self.zone.run(() => {
                    self.isLogged = isLogged;
                    self.user = self.authService.getUser();
                });
            });
    };
}
