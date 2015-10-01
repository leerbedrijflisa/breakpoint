﻿import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class dashboard {
    constructor() {
        this.isVisible = false;
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')});
    }

    activate() {
        this.loading = true;
        return this.http.get("reports").then( response => {
            this.reports = response.content;
            console.log(response.content);
            console.log(response.statusCode); // Might come in handy
            this.loading = false;
        });
    }
    submit() {
        var data = {
            Status: this.status,
        };
        console.log(this.status);
        this.http.post('reports/patch', data).then( response => {
            window.location.replace("http://localhost:10874/#/dashboard");
            this.loading = false;
        });
    }
}