import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-http-client';

export class report {
    constructor() {
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')
        });
    }

    report() {
        var report = {
            Project: {
                Slug: this.project,
                Name: this.project
            },
            StepByStep: this.stepbystep,
            Expectation: this.expectation,
            WhatHappened: this.whathappened,
            Reporters: this.reporters,
            Reported: this.reported,
            Status: this.status,
            Priority: this.priority,
            AssignedTo: this.assignedto,
            Comment: {
                comments: this.comments
            }
        }

        console.log(report);

        this.http.post('reports/insert', report).then( response => {
            this.reports = response.content;
            console.log(response.content);
            console.log(response.statusCode); // Might come in handy
            this.loading = false;
        });
    }
}