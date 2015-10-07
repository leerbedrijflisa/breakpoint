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
            this.router.navigateToRoute("organizations");
        }
    }

    Post() {
        var data = {
            userName: this.userNameRegister,
            fullName: this.fullName,
            role: this.role
        }

        this.http.post('users/post', data).then( response => {
            this.router.navigateToRoute("organizations");
        });
    }

    Login() {
        if (!readCookie("userName")) {
            var data = {
                userNameLogin: this.userNameLogin
            }

            this.http.get('users/login/'+data.userNameLogin).then( response => {
                setCookie("userName", response.content.username, 2);
                setCookie("role", response.content.role, 2);
                this.router.navigateToRoute("organizations");
            });
        }
    }
}