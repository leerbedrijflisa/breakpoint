import {HttpClient} from 'aurelia-http-client';
import {Router} from 'aurelia-router';

export class ReportData {
    static inject() {
        return [ HttpClient, Router ];
    }

    constructor(http, router) {
        this.http = http;
        this.router = router;
    }

    getAllProjects(params, user) {        
        return this.http.get('projects/'+params.organization+'/'+params.project+'/'+user);
    }
    getAllReports(params) {
        return this.http.get("reports/"+params.organization+"/"+params.project+"/"+readCookie("userName")).then( response => {
            this.reports = this.showAssigned(response.content);
        });
    }
    postReport(params, report) {
        this.http.post('reports/'+this.params.organization+'/'+this.params.project, JSON.stringify(this.report)).then(response => {
            var organization = this.params.organization;
            var project = this.params.project;

            this.router.navigateToRoute("reports", { organization: organization, project, project });
        });
    }
}