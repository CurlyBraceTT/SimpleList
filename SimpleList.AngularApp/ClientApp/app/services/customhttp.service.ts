import { Injectable } from '@angular/core';
import { Http, XHRBackend, RequestOptions, Request, RequestOptionsArgs, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import { AuthService } from '../services/auth.service';
import 'rxjs/add/observable/throw';

@Injectable()
export class CustomHttpService extends Http {
    private authService: AuthService;
    private router: Router;

    constructor(backend: XHRBackend, defaultOptions: RequestOptions, authService: AuthService, router: Router) {
        super(backend, defaultOptions);

        this.authService = authService;
        this.router = router;
    };

    get(url: string, options?: RequestOptionsArgs): Observable<Response> {
        if (!this.authService.verifyLogin()) {
            this.router.navigate(['login']);
            return Observable.throw("unauthorized");
        }

        options = this.appendAuthorizationHeader(options);
        return super.get(url, options);
    };

    post(url: string, body: any, options?: RequestOptionsArgs): Observable<Response> {
        if (!this.authService.verifyLogin()) {
            this.router.navigate(['login']);
            return Observable.throw("unauthorized");
        }

        options = this.appendAuthorizationHeader(options);
        return super.post(url, options);
    };

    put(url: string, body: any, options?: RequestOptionsArgs): Observable<Response> {
        if (!this.authService.verifyLogin()) {
            this.router.navigate(['login']);
            return Observable.throw("unauthorized");
        }

        options = this.appendAuthorizationHeader(options);
        return super.put(url, options);
    };

    delete(url: string, options?: RequestOptionsArgs): Observable<Response> {
        if (!this.authService.verifyLogin()) {
            this.router.navigate(['login']);
            return Observable.throw("unauthorized");
        }

        options = this.appendAuthorizationHeader(options);
        return super.delete(url, options);
    };

    private appendAuthorizationHeader(options?: RequestOptionsArgs): RequestOptionsArgs {
        options = options || {};
        options.headers = options.headers || new Headers();
        options.headers.append('Authorization', 'bearer ' + this.authService.getToken());
        return options;
    };
}