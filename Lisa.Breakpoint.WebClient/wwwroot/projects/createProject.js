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
    }

    create() {
        var data = {
            name: this.name,
            slug: this.name.replace(/\s+/g, '-').toLowerCase(),
            organization: this.params.organization,
            members: [
                {
                    role: 'manager',
                    userName: readCookie("userName")
                }
            ],
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