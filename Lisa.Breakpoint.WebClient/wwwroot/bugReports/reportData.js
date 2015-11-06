import {HttpClient} from 'aurelia-http-client';

export class ReportData {
    static inject() {
        return [ HttpClient ];
    }

    constructor(http) {
        this.http = http;
    }

    getAllProjects(params, user) {        
        return this.http.get('projects/'+params.organization+'/'+params.project+'/'+user).then(response => {
            this.projMembers = response.content.members;
            this.groups = response.content.groups;
            this.browsers = response.content.browsers;
        });
    }
    getAllReports(params) {
        return this.http.get("reports/"+params.organization+"/"+params.project+"/"+readCookie("userName")).then( response => {
            this.reports = this.showAssigned(response.content);
        });
    }
}