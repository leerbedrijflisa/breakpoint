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
            platforms: [],
            version: "",
            assignedTo: {
                type: "",
                value: ""
            }
        };
    }

    getPlatforms() {
        var platform = new Array();
        var meep = document.getElementsByClassName("platform");
        for (var i = 0; i < meep.length; i++) {
            platform.push(meep[i].value);
        }
        return platform;
    }

    submit() {
        this.report.platforms = this.getPlatforms();
        this.report.assignedTo.type = getAssignedToType(document.getElementById("assignedTo"));;
        this.data.postReport(this.report).then(response => {
            this.router.navigateToRoute("reports", { organization: this.report.organization, project: this.report.project });
        });
    }
}