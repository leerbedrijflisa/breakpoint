import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class Create {
    static inject() {
        return [ Router ];
    }

    constructor(router) {
        this.router = router;
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

        this.http.post('reports', data).then(response => {
            this.router.navigateToRoute("dashboard");
        });
    }
}