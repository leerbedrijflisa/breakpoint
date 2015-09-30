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
            title: this.title,
            project: {
                slug: this.project,
                name: this.project
            },
            stepByStep: this.stepbystep,
            expectation: this.expectation,
            whatHappened: this.whathappened,
            reporter: {
                userName: this.reporter,
                fullName: this.reporter,
            },
            status: this.status,
            priority: this.priority,
            assignedTo: {
                userName: this.assignedto,
                fullName: this.assignedto
            }
        }

        this.http.post('reports/patch', data).then( response => {
            window.location.replace("http://localhost:10874/#/dashboard");
            this.loading = false;
        });
    }
}