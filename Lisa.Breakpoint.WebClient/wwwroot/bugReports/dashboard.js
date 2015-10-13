﻿import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class dashboard {
    static inject() {
        return [ Router ];
    }


    constructor(router) {
        this.router = router;
        this.isVisible = false;
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')});
    }

    activate(params) {
        this.status = [];
        this.http.get('projects/get/'+params.project).then(response => {
            this.members = response.content.members;
            this.browsers = response.content.browsers;
        });
        return this.http.get("reports/"+params.project+"/"+readCookie("userName")).then( response => {
            this.reports = response.content;
            this.params = params;
            this.userName = readCookie("userName");
        });
    }

    submit(id, index) {
        var data = {
            status: this.status[index]
        };
        console.log(data.status);
        this.http.post('reports/patch/'+id, data).then( response => {
            window.location.reload();
        });
    }
}