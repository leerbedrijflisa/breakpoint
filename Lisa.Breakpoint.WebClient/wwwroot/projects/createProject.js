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
        this.http.get('users/users').then(response => {
            this.users = response.content;
        });
    }

    create() {
        var data = {
            name: this.name,
            slug: this.name,
            organization: this.params.organization,
            members: getSelectValues(document.getElementById("membersSelect"))
        };

        this.http.post('projects', data).then( response => {
            var organization = this.params.organization;
            this.router.navigateToRoute("projects", { organization: organization });
        });
    }

}