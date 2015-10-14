import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class report {
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
        this.http.get('users').then(response => {
            this.users = response.content;
        });
        this.http.get('users/groups').then(response => {
            this.groups = response.content;
        });
        return this.http.get("reports/"+params.id).then( response => {
            this.report = response.content;
            console.log(this.report.type);
        });
    }

    submit() {
        this.http.patch('reports/'+this.params.id, this.report).then( response => {
            this.router.navigateToRoute("reports", { organization: this.params.organization, project: this.params.project });
        });
    }
}