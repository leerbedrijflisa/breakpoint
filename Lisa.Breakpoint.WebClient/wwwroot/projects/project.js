﻿import {HttpClient} from 'aurelia-http-client';

export class project {
    constructor() {
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')});
    }

    activate(params) {
        this.http.get('organizations/members/'+params.organization).then(response => {
            this.members = response.content;
        });
        return this.http.get("projects/"+params.organization+'/'+readCookie("userName")).then( response => {
            this.projects = response.content;
            this.organization = params.organization;
        });
    }
}