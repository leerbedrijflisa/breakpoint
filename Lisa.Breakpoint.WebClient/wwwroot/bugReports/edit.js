import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class report {
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
        return this.http.get("reports/"+params.id).then( response => {
            this.report = response.content;
        });
    }

    submit() {
        var select = document.getElementById("assignedTo");
        if (select.options[select.selectedIndex].parentNode.label == "Groups") {
            this.report.assignedTo.type = "group";
        } else if (select.options[select.selectedIndex].parentNode.label == "Members") {
            this.report.assignedTo.type = "person";
        }
        this.report.browsers = getSelectValues(document.getElementById("browserSelect"));

        this.http.patch('reports/'+this.params.id, this.report).then( response => {
            this.router.navigateToRoute("reports", { organization: this.params.organization, project: this.params.project });
        });
    }
}