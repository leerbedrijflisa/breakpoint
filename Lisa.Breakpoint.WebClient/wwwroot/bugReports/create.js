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

        this.http.get('projects/get/'+params.project+'/'+readCookie("userName")).then(response => {
            this.projMembers = response.content.members;
            this.groups = response.content.groups;
            this.browsers = response.content.browsers;
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
            browsers: [],
            version: "",
            assignedTo: {
                type: "",
                value: ""
            }
        };
    }

    submit() {
        
        this.report.assignedTo.type = getAssignedToType(document.getElementById("assignedTo"));;
        this.report.browsers = getSelectValues(document.getElementById("browserSelect"));

        this.http.post('reports/'+this.params.project, JSON.stringify(this.report)).then(response => {
            var organization = this.params.organization;
            var project = this.params.project;

            this.router.navigateToRoute("reports", { organization: organization, project, project });
        });
    }
}