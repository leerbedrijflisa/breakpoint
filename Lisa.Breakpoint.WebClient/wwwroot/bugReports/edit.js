import {ReportData} from './reportData';
import {Router} from 'aurelia-router';

export class dashboard {
    static inject() {
        return [ ReportData, Router ];
    }

    constructor(reportData, router) {
        this.data = reportData;
        this.router = router;
    }

    activate(params) {
        this.params = params;
        this.data.getProject(params, readCookie("userName")).then(response => {
            this.projMembers = response.content.members;
            this.groups = response.content.groups;
            this.browsers = response.content.browsers;
        });
        this.data.getSingleReport(params).then( response => {
            this.report = response.content;
        });
    }

    submit() {
        this.report.assignedTo.type = getAssignedToType(document.getElementById("assignedTo"));;
        this.report.browsers = getSelectValues(document.getElementById("browserSelect"));


        this.data.patchReport(params.id, this.report).then( response => {
            this.router.navigateToRoute("reports", { organization: params.organization, project: params.project });
        });
    }
}