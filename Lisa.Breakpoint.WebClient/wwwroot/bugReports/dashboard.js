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
        this.loggedUser = readCookie("userName");
        this.data.getGroupFromUser(params, this.loggedUser).then(response => {
            this.loggedUserRole = response.content;
        });

        return Promise.all([
            this.data.getProject(params, readCookie("userName")).then(response => {
                this.members = response.content.members;
                this.browsers = response.content.browsers;
                this.groups = response.content.groups;
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
        var filters = document.getElementsByClassName('filterItem');
        var filter = "";
        var value = "";

        for (var i = filters.length - 1; i >= 0; i--)
        {
            var filterType = filters[i].id;
            filter += filters[i].id+"&";
            value += getSelectValue(filterType)+"&";
        }

        filter = filter.slice(0, -1);
        value  = value.slice(0, -1);

        this.data.getFilteredReports(this.params, readCookie("userName"), filter, value)
            .then(response => this.reports = response.content);
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
        this.data.patchReport(id, data).then( response => {
            window.location.reload();
        });
    }
}