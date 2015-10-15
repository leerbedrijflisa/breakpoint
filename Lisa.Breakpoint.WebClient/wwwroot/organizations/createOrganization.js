import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class Create {
    static inject() {
        return [ Router, HttpClient ];
    }

    constructor(router, http) {
        this.router = router;
        this.http = http;
    }

    activate() {
        return this.http.get('users').then(response => {
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