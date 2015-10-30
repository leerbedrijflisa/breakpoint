import {HttpClient} from 'aurelia-http-client';

export class project {
    static inject() {
        return [ HttpClient ];
    }

    constructor(http) {
        this.http = http;
    }

    activate(params) {
        return this.http.get("projects/"+params.organization+'/'+readCookie("userName")).then( response => {
            this.projects = response.content;
            this.organization = params.organization; 
        });
    }
}