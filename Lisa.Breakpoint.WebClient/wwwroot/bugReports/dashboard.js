﻿import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class dashboard {
    constructor() {
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')});
    }

    activate(params) {
        return this.http.get("reports/"+params.project+"/"+readCookie("userName")).then( response => {
            this.reports = response.content;
            this.organization = params.organization;
            this.project = params.project;
            this.userName = readCookie("userName");
        });
    }
}