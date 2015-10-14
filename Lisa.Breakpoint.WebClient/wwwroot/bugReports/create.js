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

        // TODO: pass entire project object to this module, so we don't have to request
        // users and groups if we already know everything about the project. Note, that
        // if you go directly to the URL for creating a report, you still need to request
        // the project from the Web API, since you don't have it yet.
        this.http.get('projects/'+params.project).then(response => {
            this.projMembers = response.content.members;
            this.groups = response.content.groups;
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
            assignedTo: {
                type: "",
                value: ""
            }
        };
    }

    submit() {
        var select = document.getElementById("assignedTo");
        this.report.assignedTo.type = select.options[select.selectedIndex].parentNode.label;

        this.http.post('reports/'+this.params.project, this.report).then(response => {
            var organization = this.params.organization;
            var project = this.params.project;

            this.router.navigateToRoute("reports", { organization: organization, project, project });
        });
    }
}