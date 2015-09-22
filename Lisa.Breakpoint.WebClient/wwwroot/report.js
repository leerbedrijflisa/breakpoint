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
            Reported: this.reported,
            Status: this.status,
            Priority: this.priority,
            AssignedTo: [
                {
                    userName: this.assignedto,
                    FullName: this.assignedto
                }
            ],
            Comments: [
                {
                    Posted: "2013-06-01T12:32:30.0000000",
                    Author: "Bas eenhoorn",
                    Text: this.comment
                }
            ]
        }

        this.http.post('reports/insert', data).then( response => {
            this.reports = response.content;
            console.log(response.content);
            console.log(response.statusCode); // Might come in handy
            this.loading = false;
        });
    }
}