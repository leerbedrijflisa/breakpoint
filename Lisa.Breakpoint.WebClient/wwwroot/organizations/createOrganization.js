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
    
    submit() {
        var data = {
            name: this.name,
            slug: this.name,
            members: [
                {
                    userName: this.members,
                    fullName: this.members
                }
            ]
        };

        this.http.post('organizations/post', data).then(response => {
            this.router.navigateToRoute("organizations");
        });
    }
}