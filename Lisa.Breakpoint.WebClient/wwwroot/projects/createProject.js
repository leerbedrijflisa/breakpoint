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
        console.log('lol');
        return this.http.get('organizations/members/'+params.organization).then(response => {
            console.log(response.content);
            this.orgMembers = response.content;
        });
    }

    create() {
        var members = getSelectValues(document.getElementById("membersSelect"));
        var memberList = [];

        members.forEach(function(member) {
            var role;

            if (member == readCookie("userName")) {
                role = 'manager'
            } else {
                role = 'developer'
            }
            var m = {
                userName: member,
                role: role
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
                {
                    "Level": 0,
                    "Name": "tester"
                },
                {
                    "Level": 1,
                    "Name": "developer"
                },
                {
                    "Level": 2,
                    "Name": "manager"
                }
            ],
            projectManager: readCookie("userName")
        };

        this.http.post('projects/'+readCookie("userName"), data).then( response => {
            this.router.navigateToRoute("projects", { organization: this.params.organization });
        });
    }

}