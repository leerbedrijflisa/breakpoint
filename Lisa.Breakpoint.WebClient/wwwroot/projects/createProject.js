import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class createProject {
    static inject() {
        return [ Router ];
    }

    constructor(router) {
        this.router = router;
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')
        });
    }

    activate(params) {
        this.params = params;
        this.http.get('users').then(response => {
            this.users = response.content;
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
            projectManager: readCookie("userName"),
            members: memberList
        };

        this.http.post('projects', data).then( response => {
            var organization = this.params.organization;
            this.router.navigateToRoute("projects", { organization: organization });
        });
    }

}