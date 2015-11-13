import {Router} from 'aurelia-router';
import {ReportData} from './reportData';

export class dashboard {
    static inject() {
        return [ReportData, Router ];
    }

    constructor(reportData, router) {
        this.data = reportData;
        this.router = router;
    }

    activate(params) {
        this.params = params;
        this.disabled = true;
        this.showAssignedTo = [];
        //You need this because "this.disabled" give a unhandled promise rejection.
        var thiss = this;
        this.data.getAllProjects(params, readCookie("userName")).then(response => {
            this.project = response.content;
            this.members = response.content.members;
            //Foreach and if to check wich user is a manager.
            this.members.forEach(function(member, i) {
                if (member.userName == readCookie("userName") && member.role == "manager") {
                    thiss.disabled = null;
                    return;
                } 
            });
        });
        return this.data.getAllReports(params, readCookie("userName")).then( response => {
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
        //A switch function that checks if the chosen status has any special attributes, more can be added in the same way as these below.
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
        this.data.patchReport(id, data).then( response => {
            window.location.reload();
        });
    }
}