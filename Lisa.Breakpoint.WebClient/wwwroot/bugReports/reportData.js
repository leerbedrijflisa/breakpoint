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
        return this.http.get('projects/'+ params.organization+'/'+ params.project+'/'+user+'/true');
    }

    // Send the Params and the user
    getGroupFromUser(params, user) {        
        return this.http.get('users/'+ params.organization+'/'+ params.project+'/'+user);
    }

    // Send the full report 
    postReport(report) {
        return this.http.post('reports/'+report.organization+'/'+report.project, JSON.stringify(report));
    }
    // Send the Id and the changed report
    patchReport(id, data) {
        return this.http.patch('reports/' + id + "/" + readCookie("userName"), data);
    }

    getPlatforms() {
        return this.http.get('platforms');
    }

}