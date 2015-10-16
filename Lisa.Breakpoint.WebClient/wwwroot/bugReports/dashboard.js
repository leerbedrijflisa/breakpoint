﻿import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class dashboard {
    static inject() {
        return [ Router, HttpClient ];
    }

    constructor(router, http) {
        this.router = router;
        this.isVisible = false;
        this.http = http;
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

    //patch status
    submit(id, index) {
        if (this.status[index] == null) {
            this.status[index] = document.getElementById("status"+id).options[0].value; 
        }
        var data = { status: this.status[index] };
        this.http.patch('reports/' + id, data).then( response => {
            window.location.reload();
        });
    }
}