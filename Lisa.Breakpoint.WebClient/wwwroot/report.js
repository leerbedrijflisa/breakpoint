﻿import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class report {
constructor() {
    this.http = new HttpClient().configure(x => {
        x.withBaseUrl('http://localhost:10791/api');      
        x.withHeader('Content-Type', 'application/json')});
    }

activate() {
    this.loading = true;
    return this.http.get("./Demo").then( response => {
        this.data = response.content;
        this.code = response.statusCode;
        this.loading = false;
    });
    }
}