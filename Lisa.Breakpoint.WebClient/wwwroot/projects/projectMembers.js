import {HttpClient} from 'aurelia-http-client';

export class project {
    constructor() {
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')});
    }

    activate(params) {
        this.params = params;
        return this.http.get('projects/'+params.project).then(response => {
            this.members = response.content.members;
            this.groups = response.content.groups;
            this.params = params;
        });
    }

    saveChanges(member) {
        var sel = document.getElementById("role_"+member);
        var role = sel.options[sel.selectedIndex].value;

        var patch = {
            type: "update",
            field: "username",
            key: member,
            value: role
        };

        //this.http.patch('projects/member/'+this.params.project, patch).then(response => {
        console.log(member+": "+role);
        console.log(patch);
        //});
    }
}