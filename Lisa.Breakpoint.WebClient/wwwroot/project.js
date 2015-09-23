﻿import {HttpClient} from 'aurelia-http-client';

export class project {
    constructor() {
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')});
    }

    activate() {
        this.loading = true;
        return this.http.get("projects").then( response => {
            this.projects = response.content;
            console.log(this.projects);
            console.log(response.content);
            console.log(response.statusCode); // Might come in handy
            this.loading = false;
        });
    }
}