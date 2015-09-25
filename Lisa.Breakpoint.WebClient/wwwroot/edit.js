import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-http-client';

export class report {
    constructor() {
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')
        });
    }

    activate() {
        this.loading = true;

        var path = "reports/609";
        return this.http.get(path).then( response => {
            this.report = response.content;
            this.loading = false;
        });
    }

    submit() {
        var data = {
            Project: [
                {
                    Id: this.project,
                    Slug: this.project,
                    Name: this.project
                }
            ],
            StepByStep: this.stepbystep,
            Expectation: this.expectation,
            WhatHappened: this.whathappened,
            Reporters: [
                {
                    userName: this.reporters,
                }
            ],
            Status: this.status,
            Priority: this.priority,
            AssignedTo: [
                {
                    userName: this.assignedto,
                    FullName: this.assignedto
                }
            ]
        }

        this.http.post('reports/patch', data).then( response => {
            window.location.replace("http://localhost:10874/#/dashboard");
            this.loading = false;
        });
    }
}