import { NgModule } from '@angular/core';
import { RouterModule, Router } from '@angular/router';
import { UniversalModule } from 'angular2-universal';
import { FormsModule } from '@angular/forms';
import { Http, XHRBackend, RequestOptions, Request, RequestOptionsArgs, Response, Headers } from '@angular/http';

import { AppComponent } from './components/app/app.component'
import { NavMenuComponent } from './components/navmenu/navmenu.component';
import { HomeComponent } from './components/home/home.component';
import { SimpleListComponent } from './components/simplelist/simplelist.component';
import { LoginComponent } from './components/login/login.component';
import { LogoutComponent } from './components/logout/logout.component';
import { ListItemComponent } from './components/listitem/listitem.component';

import { ListItemsService } from './services/listitems.service';
import { AuthService } from './services/auth.service';
import { CustomHttpService } from './services/customhttp.service';
import { AuthGuardService } from './services/authguard.service';

@NgModule({
    bootstrap: [ AppComponent ],
    declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        SimpleListComponent,
        ListItemComponent,
        LoginComponent,
        LogoutComponent,
    ],
    imports: [
        UniversalModule, // Must be first import. This automatically imports BrowserModule, HttpModule, and JsonpModule too.
        FormsModule,
        RouterModule.forRoot([
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            { path: 'home', component: HomeComponent },
            { path: 'list', component: SimpleListComponent, canActivate: [AuthGuardService] },
            { path: 'login', component: LoginComponent },
            { path: '**', redirectTo: 'home' }
        ])
    ],
    providers: [
        AuthService,
        AuthGuardService,
        ListItemsService,
        {
            provide: CustomHttpService,
            useFactory: (backend: XHRBackend, defaultOptions: RequestOptions, authService: AuthService, router: Router) =>
                new CustomHttpService(backend, defaultOptions, authService, router),
            deps: [XHRBackend, RequestOptions, AuthService, Router]
        }
    ]
})
export class AppModule {
}
