﻿import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class dashboard {
    constructor() {
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')});
    }

    activate() {
        this.loading = true;
        return this.http.get("reports").then( response => {
            this.reports = response.content;

            // this.reports.idNumber = this.reports.Id.split("/")[1];


            console.log(response.content);
            console.log(response.statusCode); // Might come in handy
            this.loading = false;
        });
    }
}