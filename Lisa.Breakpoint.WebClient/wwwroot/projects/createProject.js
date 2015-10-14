import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class createProject {
    static inject() {
        return [ Router, HttpClient ];
    }

    constructor(router, http) {
        this.router = router;
        this.http = http;
    }

    activate(params) {
        this.params = params;
        return this.http.get('organizations/members/'+params.organization).then(response => {
            this.orgMembers = response.content;
        });
    }

    create() {
        var members = getSelectValues(document.getElementById("membersSelect"));
        var memberList = [];

        members.forEach(function(member) {
            var m = {
                userName: member,
                role: ''
            };
            memberList.push(m);
        });

        var data = {
            name: this.name,
            slug: this.name.replace(/\s+/g, '-').toLowerCase(),
            organization: this.params.organization,
            members: memberList,
            browsers: getSelectValues(document.getElementById("browserSelect")),
            groups: [
                "tester",
                "developer",
                "manager"
            ],
            projectManager: readCookie("userName")
        };

        this.http.post('projects', data).then( response => {
            var organization = this.params.organization;
            this.router.navigateToRoute("projects", { organization: organization });
        });
    }

}