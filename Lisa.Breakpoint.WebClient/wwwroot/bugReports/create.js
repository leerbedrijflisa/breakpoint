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

    activate() {
        this.loading = true;
        return this.http.get("projects").then( response => {
            this.projects = response.content;

            console.log(response.content);
            console.log(response.statusCode); // Might come in handy
            
            this.loading = false;
        });
    }

    submit() {
        console.log("hengel");


        var data = {
            project: {
                id: this.project.id,
                slug: this.project.name,
                name: this.project.slug
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
            },
            version: this.version
        };

        this.http.post('reports', data).then(response => {
            this.report = response.content;
            this.router.navigateToRoute("report");
        });
    }
}