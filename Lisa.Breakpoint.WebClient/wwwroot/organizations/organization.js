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
        return this.http.get("organizations/"+readCookie("userName")).then( response => {
            this.organizations = response.content;
            this.userName = readCookie("userName");
        });
    }
}