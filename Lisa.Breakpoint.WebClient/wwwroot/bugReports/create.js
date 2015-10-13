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

        // TODO: pass entire project object to this module, so we don't have to request
        // users and groups if we already know everything about the project. Note, that
        // if you go directly to the URL for creating a report, you still need to request
        // the project from the Web API, since you don't have it yet.
        this.http.get('users/users').then(response => {
            this.users = response.content;
        });
        this.http.get('users/groups').then(response => {
            this.groups = response.content;
        });

        this.report = {
            title: "",
            project: params.project,
            stepByStep: "",
            expectation: "",
            whatHappened: "",
            reporter: readCookie("userName"),
            status: "Open",
            priority: "fix immediately",
            assignedTo: "person",
            assignedToPerson: null,
            assignedToGroup: null
        };
    }

    submit() {
         // TODO: check if assignedTo still works correctly now that RAJ removed the code.

        this.http.post('reports', this.report).then(response => {
            var organization = this.params.organization;
            var project = this.params.project;

            this.router.navigateToRoute("reports", { organization: organization, project, project });
        });
    }
}