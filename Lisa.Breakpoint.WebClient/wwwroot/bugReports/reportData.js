import {HttpClient} from 'aurelia-http-client';

export class ReportData {
    static inject() {
        return [ HttpClient];
    }

    constructor(http) {
        this.http = http;
    }

    // Send the Params and the user
    getAllReports(params, user){
        return this.http.get("reports/"+params.organization+"/"+params.project+"/"+readCookie("userName"));
    }

    // Send the Params, filter and the user
    getFilteredReports(params, user, filter, value){
        return this.http.get("reports/"+params.organization+"/"+params.project+"/"+readCookie("userName")+"/"+filter+"/"+value)
    }

    // Send the Params and the user
    getProject(params, user) {        
        return this.http.get('projects/'+ params.organization+'/'+ params.project+'/'+user);
    }

    // Send the full report 
    postReport(report) {
        this.report = report;
        return this.http.post('reports/'+this.report.organization+'/'+this.report.project, JSON.stringify(this.report));
    }
    // Send the Id and the changed report
    patchReport(id, data) {
        return this.http.patch('reports/' + id, data);
    }

}