import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-http-client';

export class createProject {
    constructor() {
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')
        });
    }

    create() {
        var data = {
            name: this.name,
            slug: this.name,
            organization: this.organization,
            members: [
                {
                    userName: this.members,
                    fullName: this.members
                }
            ]
        };

        this.http.post('projects/insert', data).then( response => {
            this.projects = response.content;
        });
    }

}