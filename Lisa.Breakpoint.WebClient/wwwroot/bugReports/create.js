import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class Create {
    static inject() {
        return [ Router, HttpClient ];
    }

    constructor(router, http) {
        this.router = router;
        this.http = http;
    }
    
    activate(params) {
        this.params = params;
        console.log(params);
        this.http.get('users/users').then(response => {
            this.users = response.content;
        });
        this.http.get('users/groups').then(response => {
            this.groups = response.content;
        });
        this.http.get('projects').then(response => {
            this.project = response.content;
        });
    }

    submit() {
        if (this.priority == null) {
            this.priority = document.getElementById("priority").options[0].value; // if the first option is selected it wont register the value so i have to get the first option myself
        }

        if (document.getElementById('personRadioButton').checked == true) {
            var data = {
                title: this.title,
                project: this.params.project,
                stepByStep: this.stepbystep,
                expectation: this.expectation,
                whatHappened: this.whathappened,
                reporter: readCookie("userName"),
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