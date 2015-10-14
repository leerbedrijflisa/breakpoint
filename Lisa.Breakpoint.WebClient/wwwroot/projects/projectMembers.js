import {HttpClient} from 'aurelia-http-client';

export class project {
    constructor() {
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')});
    }

    saveChanges(user, val) {
        console.log(user + ': ' + val);
    }

    activate(params) {
        return this.http.get('projects/members/'+params.project).then(response => {
            this.members = response.content;
            this.params = params;
        });
    }
}