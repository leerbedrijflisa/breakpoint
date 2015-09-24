import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-http-client';

export class user {
    constructor() {
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')
        });
    }

    Post() {
        var data = {
            userName: this.userName,
            fullName: this.fullName,
            role: this.role
        }

        this.http.post('users/insert', data).then( response => {
            this.reports = response.content;
            console.log(response.content);
            console.log(response.statusCode); // Might come in handy
        });
    }
}