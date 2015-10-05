import {Router} from 'aurelia-router';
import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-http-client';

export class user {
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
        if (readCookie("userName")) {
            this.router.navigateToRoute("dashboard");
        }
    }

    Post() {
        if (!readCookie("userName")) {
            var data = {
                userName: this.userName,
                fullName: this.fullName,
                role: this.role
            }

            //setCookie("userName", this.userName, 2);

            this.http.post('users/insert', data).then( response => {
                this.router.navigateToRoute("dashboard");
            });
        }
    }
}