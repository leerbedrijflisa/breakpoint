import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class report {
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

    activate(params) {
        this.params = params;
        var path = "reports/get/"+params.id;
        return this.http.get(path).then( response => {
            this.report = response.content;
        });
    }

    submit() {
        var data = {
            title: this.title,
            project: this.project,
            stepByStep: this.stepbystep,
            expectation: this.expectation,
            whatHappened: this.whathappened,
            reporter: this.reporter,
            status: this.status,
            priority: this.priority,
            assignedTo: this.assignedTo
        }

        this.http.post('reports/patch/'+this.params.id, data).then( response => {
            this.router.navigateToRoute("reports", { organization: this.params.organization, project: this.params.project });
        });
    }
}