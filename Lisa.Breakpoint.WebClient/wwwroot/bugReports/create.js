import {HttpClient} from 'aurelia-http-client';
import {ReportData} from './reportData';

export class Create {
    static inject() {
        return [ HttpClient, ReportData ];
    }

    constructor(http, reportData) {
        this.http = http;
        this.data = reportData;
    }
    
    activate(params) {
        this.data.getAllProjects(params, readCookie("userName"))
            .then(response => {
                this.projMembers = response.content.members;
                this.groups = response.content.groups;
                this.browsers = response.content.browsers;
            });
        this.report = {
            title: "",
            project: params.project,
            organization: params.organization,
            stepByStep: "",
            expectation: "",
            whatHappened: "",
            reporter: readCookie("userName"),
            status: "Open",
            priority: 0,
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

        this.http.post('reports/'+this.params.organization+'/'+this.params.project, JSON.stringify(this.report)).then(response => {
            var organization = this.params.organization;
            var project = this.params.project;

            this.router.navigateToRoute("reports", { organization: organization, project, project });
        });
    }
}