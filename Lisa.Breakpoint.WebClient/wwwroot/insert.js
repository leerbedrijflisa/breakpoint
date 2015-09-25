import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-http-client';

export class report {
    constructor() {
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')
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
                    FullName: this.reporters
                }
            ],
            Status: "Open",
            Priority: this.priority,
            AssignedTo: [
                {
                    userName: this.assignedto,
                }
            ]
        };

        console.log(data);

        this.http.post('reports/post', data).then( response => {
            window.location.replace("http://localhost:10874/#/dashboard");
            this.loading = false;
        });
    }
}