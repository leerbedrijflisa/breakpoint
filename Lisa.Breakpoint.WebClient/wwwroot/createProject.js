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
            Name: this.projectName,
            Slug: this.projectName.replace(/\s+/g, '-').toLowerCase(),
            Member: [
                {
                    Role: "ProjectManager",
                    UserName: "Leon",
                    FullName: "Leon Tribe"
                }
            ]
        }

        console.log(data);

        this.http.post('projects/insert', data).then( response => {
            this.projects = response.content;
            console.log(response.content);
            console.log(response.statusCode); // Might come in handy
            this.loading = false;
        });
        window.location.href = '#/project';
    }

}