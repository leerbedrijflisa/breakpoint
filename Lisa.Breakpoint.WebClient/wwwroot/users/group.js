import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class Create {
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
        this.http.get('users/groups').then(response => {
            this.groups = response.content;
        });
    }

    submit() {
        var data = {
            name: this.name,
            members: [
                {
                    fullName: this.members
                }
            ]
        }

        this.http.post('users/groups', data).then(response => {
            this.router.navigateToRoute("groups");
        });
    }
}