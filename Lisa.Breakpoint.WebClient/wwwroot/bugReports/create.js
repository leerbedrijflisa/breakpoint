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
    
    activate(params) {
        this.params = params;
    }

    submit() {
        if (document.getElementById('personRadioButton').checked == true) {
            var data = {
                title: this.title,
                project: this.params.project,
                stepByStep: this.stepbystep,
                expectation: this.expectation,
                whatHappened: this.whathappened,
                reporter: this.reporter,
                status: "Open",
                priority: this.priority,
                assignedTo: "person",
                assignedToPerson: this.assignedtoperson
            }
        } else {
            var data = {
                title: this.title,
                project: this.params.project,
                stepByStep: this.stepbystep,
                expectation: this.expectation,
                whatHappened: this.whathappened,
                reporter: this.reporter,
                status: "Open",
                priority: this.priority,
                assignedTo: "group",
                assignedToGroup: this.assignedtogroup
            }
        }

        this.http.post('reports', data).then(response => {
            var organization = this.params.organization;
            var project = this.params.project;

            this.router.navigateToRoute("reports", { organization: organization, project, project });
        });
    }
}