﻿import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class dashboard {
    static inject() {
        return [ Router, HttpClient ];
    }

    constructor(router, http) {
        this.router = router;
        this.isVisible = false;
        this.http = http;
    }

    activate(params) {
        this.showAssignedTo = [];
        this.userName = readCookie("userName");
        this.params = params;

        this.http.get('projects/'+params.organization+'/'+params.project+'/'+readCookie("userName")).then(response => {
            this.members = response.content.members;
        });
        return this.http.get("reports/"+params.organization+"/"+params.project+"/"+readCookie("userName")).then( response => {
            this.reports = this.showAssigned(response.content);
        });
    }

    showAssigned(reports) {
        var reportsLength= 0;
        var i;
        for(var key in reports) {
            if(reports.hasOwnProperty(key)){
                reportsLength++;
            }
        }
        for (i = 0; i < reportsLength; i++) {
            if (reports[i].assignedTo.type == "") {
                this.showAssignedTo[i] = false;
            } else {
                this.showAssignedTo[i] = true;
            }
        }
        return reports;
    }

    patchStatus(id, index) {
        if (this.reports[index].status == null) {
            this.reports[index].status = document.getElementById("status"+id).options[0].value; 
        };
        switch (this.reports[index].status) {
            case "Fixed":
                var data = {
                    status: this.reports[index].status,
                    assignedTo: {
                        type: "group",
                        value: "tester"
                    }
                };
                break;
            case "Closed":
                var data = {
                    status: this.reports[index].status,
                    assignedTo: {
                        type: "",
                        value: ""
                    }
                };
                break;
            default:
                var data = {
                    status: this.reports[index].status
                };
                break;
        }

        this.http.patch('reports/' + id, data).then( response => {
            window.location.reload();
        });
    }
}