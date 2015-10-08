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

    activate() {
        this.http.get('users/users').then(response => {
            this.users = response.content;
        });
    }
    
    submit() {
        var data = {
            name: this.name,
            slug: this.slug,
            members: getSelectValues(document.getElementById("membersSelect"))
        };

        this.http.post('organizations/post', data).then(response => {
            this.router.navigateToRoute("organizations");
        });
    }
}