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
        this.group = params.group;
        this.http.get('users/groups/'+params.group).then(response => {
            this.members = response.content;
        });
    }
}