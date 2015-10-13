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
        this.http.get('projects/members/'+params.project).then(response => {
            this.members = response.content;
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
        this.http.patch('reports/' + id, data).then( response => {
            window.location.reload();
        });
    }
}