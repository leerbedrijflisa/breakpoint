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
        this.closedDisabled = null;
        this.wontfixDisabled = true;
        this.showAssignedTo = [];
        this.loggedUser = readCookie("userName");

        return Promise.all([
            this.data.getGroupFromUser(params, this.loggedUser).then(response => {
                this.loggedUserRole = response.content;
                if (this.loggedUserRole == "manager") {
                    this.wontfixDisabled = null;
                }
                if (this.loggedUserRole == "developer") {
                    this.closedDisabled = true;
                }
                this.firstFilter = "member&group";
                this.firstValues = this.loggedUser+"&"+this.loggedUserRole;

                this.data.getFilteredReports(params, this.loggedUser, this.firstFilter, this.firstValues).then( response => {
                    this.reports = this.showAssigned(response.content);
                    this.versions = this.getTestVersions(this.reports);
                    this.reportsCount = count(this.reports);
                })
            }),
            this.data.getProject(params, this.loggedUser).then(response => {
                this.members = response.content.members;
                this.groups = response.content.groups;
            })
        ]);
    }
    showAssigned(reports) {
        var reportsLength = count(reports);
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
        var reportsLength = count(reports);
        var versions = [];

        var i;
        for (i = 0; i < reportsLength; i++) {
            if (reports[i].version != "") {
                versions.push(reports[i].version);
            }
        }

        return versions.filter(function(item, pos) {
            return versions.indexOf(item) == pos;
        })
    }

    filterReports() {
        var filters = document.getElementsByClassName('filterItem');
        var filter = "";
        var value = "";

        for (var i = filters.length - 1; i >= 0; i--)
        {
            var filterType = filters[i].id;
            if (filterType == "titleFilter") {
                if (filters[i].value == "") {
                    continue;
                } else {
                    filter += filters[i].id+"&";
                    value += filters[i].value+"&";
                }
            } else {
                filter += filters[i].id+"&";
                value += getSelectValue(filterType)+"&";
            }
        }

        filter = filter.slice(0, -1);
        value  = value.slice(0, -1);

        return this.data.getFilteredReports(this.params, readCookie("userName"), filter, value).then(response => {
            this.reports = response.content;
            this.reportsCount = count(this.reports);
        });
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