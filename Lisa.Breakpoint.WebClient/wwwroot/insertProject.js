import {inject} from 'aurelia-framework';
import {HttpClient} from 'aurelia-http-client';

export class report {
    constructor() {
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')
        });
    }

    submit() {
        var data = {
            Slug: this.slug,
            Name: this.name,
            Members: [
                {
                    Role: this.members,
                    UserName: this.members,
                    FullName: this.members
                }
            ]
        };

        console.log(data);

        this.http.post('projects/post', data).then( response => {
            window.location.replace("http://localhost:10874/#/dashboard");
            this.loading = false;
        });
    }
}