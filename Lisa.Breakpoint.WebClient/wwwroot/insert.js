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
            project: {
                slug: this.project,
                name: this.project
            },
            stepByStep: this.stepbystep,
            expectation: this.expectation,
            whatHappened: this.whathappened,
            reporter: {
                userName: this.reporters,
                fullName: this.reporters
            },
            status: "Open",
            priority: this.priority,
            assignedTo: {
                userName: this.assignedto,
            }
        };

        console.log(data);

        this.http.post('reports', data).then( response => {
            window.location.replace("http://localhost:10874/#/dashboard");
            this.loading = false;
        });
    }
}