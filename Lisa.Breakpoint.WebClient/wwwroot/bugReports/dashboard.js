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
        this.userName = readCookie("userName");
        this.params = params;

        this.http.get('projects/'+params.organization+'/'+params.project+'/'+readCookie("userName")).then(response => {
            this.members = response.content.members;
            this.browsers = response.content.browsers;
        });
        return this.http.get("reports/"+params.organization+"/"+params.project+"/"+readCookie("userName")).then( response => {
            this.reports = response.content;
            var members = this.members;
            var userRole;
            this.authorized = Array;
            var thiss = this;

            //checks if the users role in a variable to use
            members.forEach(function(member){
                if(readCookie("userName") == member.userName){
                    var userRole = member.role;
                    return;
                }
            });
            
            //if the user is a developer than it makes it so he'll be authorized to be able to close an report he made
            if (userRole = "developer") {
                this.reports.forEach(function(report, i){
                    if (report.reporter == readCookie("userName")) {
                        thiss.authorized[i] = null;
                    } else {
                        thiss.authorized[i] = true;
                    }
                });
            }
        });
    }

    patchStatus(id, index) {
        if (this.reports[index].status == null) {
            this.reports[index].status = document.getElementById("status"+id).options[0].value; 
        };
        var data = {
            status: this.reports[index].status
        };

        this.http.patch('reports/' + id, data).then( response => {
            window.location.reload();
        });
    }
}