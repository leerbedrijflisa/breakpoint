import {Router} from 'aurelia-router';
import {ReportData} from './reportData';

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
        this.showAssignedTo = [];

        return Promise.all([
            this.data.getAllProjects(params, readCookie("userName")).then(response => {
                this.members = response.content.members;
                this.browsers = response.content.browsers;
            }),
            this.data.getAllReports(params, readCookie("userName")).then( response => {
                this.reports = this.showAssigned(response.content);
                this.versions = this.getTestVersions(this.reports);
            })
        ]);
    }

    showAssigned(reports) {
        var reportsLength= 0;
        for(var key in reports) {
            if(reports.hasOwnProperty(key)){
                reportsLength++;
            }
        }

        var i;
        for (i = 0; i < reportsLength; i++) {
            if (reports[i].assignedTo.type == "") {
                this.showAssignedTo[i] = false;
            } else {
                this.showAssignedTo[i] = true;
            }
        }
        return reports;
    }

    getTestVersions(reports) {
        var reportsLength = 0;
        var versions = [];

        for(var key in reports) {
            if(reports.hasOwnProperty(key)){
                reportsLength++;
            }
        }

        var i;
        for (i = 0; i < reportsLength; i++) {
            if (reports[i].version != "") {
                versions.push(reports[i].version);
            }
        }

        var uniqueVersions = versions.filter(function(item, pos) {
            return versions.indexOf(item) == pos;
        })

        return uniqueVersions;
    }

    filterReports() {
        var el = document.getElementById("version")
        if (typeof(el) != 'undefined' && el != null) {
            var version = getSelectValue("version");
            this.data.getFilteredReports(this.params, readCookie("userName"), "version", version)
                .then(response => this.reports = response.content);
        }
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