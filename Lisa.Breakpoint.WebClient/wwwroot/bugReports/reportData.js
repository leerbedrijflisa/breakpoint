import {HttpClient} from 'aurelia-http-client';

export class ReportData {
    static inject() {
        return [ HttpClient];
    }

    constructor(http) {
        this.http = http;
    }

    // Sent the Params and the user
    getAllReports(params, user){
        return this.http.get("reports/"+params.organization+"/"+params.project+"/"+readCookie("userName"));
    }

    // Sent the Params and the user
    getAllProjects(params, user) {        
        return this.http.get('projects/'+ params.organization+'/'+ params.project+'/'+user);
    }

    // Sent the full report 
    postReport(report) {
        this.report = report;
        return this.http.post('reports/'+this.report.organization+'/'+this.report.project, JSON.stringify(this.report));
    }
    //Sent the Id and the changed report
    patchReport(id, data) {
        return this.http.patch('reports/' + id, data);
    }

}