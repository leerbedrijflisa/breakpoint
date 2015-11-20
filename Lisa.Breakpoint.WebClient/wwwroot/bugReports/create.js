import {ReportData} from './reportData';
import {Router} from "aurelia-router";

export class Create {
    static inject() {
        return [ReportData, Router ];
    }

    constructor(reportData, router) {
        this.data = reportData;
        this.router = router;
    }
    
    activate(params) {
        this.data.getProject(params, readCookie("userName")).then(response => {
            this.projMembers = response.content.members;
            this.groups = response.content.groups;
        });

        this.data.getPlatforms().then(response => {
            this.platforms = response.content.sort();
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
            platform: "",
            version: "",
            assignedTo: {
                type: "",
                value: ""
            }
        };
    }

    submit() {
        this.report.assignedTo.type = getAssignedToType(document.getElementById("assignedTo"));;
        this.data.postReport(this.report).then(response => {
            this.router.navigateToRoute("reports", { organization: this.report.organization, project: this.report.project });
        });
    }
}