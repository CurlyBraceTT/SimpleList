import { Injectable } from '@angular/core';
import { Subject } from 'rxjs/Subject';
import { Headers, Http } from '@angular/http';

@Injectable()
export class AuthService {
    private loginUrl = 'https://localhost:44302/api/auth/login';
    private loggedFlag: boolean = false;

    loggedSubject$: Subject<boolean> = new Subject<boolean>();

    get isLoggedIn(): boolean {
        return this.loggedFlag;
    };

    constructor(private http: Http) {
        this.verifyLogin();
    };

    verifyLogin(): boolean {
        var token = localStorage.getItem('access_token');
        var expiration = localStorage.getItem('expiration');

        if (token != null && expiration != null) {
            var nowUtc = new Date().getTime();
            var expirationDate = new Date(expiration).getTime();

            if (expirationDate < nowUtc) {
                this.logout();
            } else {
                this.loggedFlag = true;
            }
        } else {
            this.loggedFlag = false;
        }

        this.loggedSubject$.next(this.loggedFlag);
        return this.loggedFlag;
    };

    googleLogin(token: string, user: string): Promise<any> {
        var headers = new Headers();
        headers.append('Authorization', token); 

        return this.http
            .post(this.loginUrl, {}, { headers: headers })
            .toPromise()
            .then(res => this.handleLogin(res.json(), user));
    };

    getToken(): string {
        return localStorage.getItem('access_token');
    };

    getUser(): string {
        return localStorage.getItem('user');
    };

    logout(): void {
        localStorage.removeItem('user');
        localStorage.removeItem('access_token');
        localStorage.removeItem('expiration');
        this.loggedFlag = false;
        this.loggedSubject$.next(this.loggedFlag);
    };

    private handleLogin(res: any, user: string): void {
        localStorage.setItem('access_token', res.token);
        localStorage.setItem('expiration', res.expiration);
        localStorage.setItem('user', user);

        this.loggedFlag = true;
        this.loggedSubject$.next(this.loggedFlag);
    };
}