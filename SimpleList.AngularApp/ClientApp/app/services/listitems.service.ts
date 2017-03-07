import { Injectable } from '@angular/core';
import { Headers, Http } from '@angular/http';
import { ListItem } from '../models/listitem.model';
import { CustomHttpService } from '../services/customhttp.service';

import 'rxjs/add/operator/toPromise';

@Injectable()
export class ListItemsService {
    private endpointUrl = 'https://localhost:44302/api/listitems';

    constructor(private http: CustomHttpService) { }

    getAll(): Promise<ListItem[]> {
        return this.http.get(this.endpointUrl)
            .toPromise()
            .then(response => response.json() as ListItem[])
            .catch(this.handleError);
    };

    get(id: number): Promise<ListItem> {
        const url = `${this.endpointUrl}/${id}`;

        return this.http.get(url)
            .toPromise()
            .then(response => response.json() as ListItem)
            .catch(this.handleError);
    };

    delete(id: number): Promise<void> {
        var headers = new Headers();
        this.appendJsonHeaders(headers);

        const url = `${this.endpointUrl}/${id}`
        return this.http.delete(url, { headers: headers })
            .toPromise()
            .then(() => null)
            .catch(this.handleError);
    };

    create(item: ListItem): Promise<ListItem> {
        var headers = new Headers();
        this.appendJsonHeaders(headers);

        return this.http
            .post(this.endpointUrl, JSON.stringify(item), { headers: headers })
            .toPromise()
            .then(res => res.json())
            .catch(this.handleError);
    };

    update(item: ListItem): Promise<ListItem> {
        var headers = new Headers();
        this.appendJsonHeaders(headers);

        const url = `${this.endpointUrl}/${item.id}`;
        return this.http
            .put(url, JSON.stringify(item), { headers: headers })
            .toPromise()
            .then(() => item)
            .catch(this.handleError);
    };

    private appendJsonHeaders(headers: Headers) {
        headers.append('Content-Type', 'application/json');
    };

    private handleError(error: any): Promise<any> {
        console.error('An error occurred', error); // for demo purposes only
        return Promise.reject(error.message || error);
    };
}