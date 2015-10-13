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

        this.http.get('users/groups').then(response => {
            this.roles = response.content;
        });
    }

    Post() {
        var data = {
            userName: this.userNameRegister,
            fullName: this.fullName
        }

        this.http.post('users', data).then( response => {
            this.Login("afterRegister");
            this.router.navigateToRoute("organizations");
        });
    }

    Login(from) {
        if (!readCookie("userName")) {
            if (from == "afterRegister") {
                var data = {
                    userNameLogin: this.userNameRegister
                }
            } else {
                var data = {
                    userNameLogin: this.userNameLogin
                }
            }

            this.http.get('users/login/'+data.userNameLogin).then( response => {
                if (response.content != null) {
                    setCookie("userName", response.content.username, 2);
                    document.getElementById("user_userName").innerHTML = "Logged in as: " + readCookie("userName");
                    this.router.navigateToRoute("organizations");
                }
            });
        }
    }
}