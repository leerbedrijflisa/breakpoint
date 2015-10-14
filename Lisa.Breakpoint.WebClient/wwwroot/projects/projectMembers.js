import {HttpClient} from 'aurelia-http-client';

export class project {
    constructor() {
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')});
    }

    activate(params) {
        return this.http.get('projects/'+params.project).then(response => {
            this.members = response.content.members;
            this.params = params;
        });
    }

    saveChanges(member) {
        var sel = document.getElementById("role_"+member);
        var role = sel.options[sel.selectedIndex].value;

        this.http.patch('projects/member/'+params.project, patch).then(response => {
            console.log(member+": "+role);
        });
    }
}