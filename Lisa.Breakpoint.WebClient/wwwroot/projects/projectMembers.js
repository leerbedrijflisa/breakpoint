import {HttpClient} from 'aurelia-http-client';

export class project {
    static inject() {
        return [ HttpClient ];
    }

    constructor(http) {
        this.http = http;
    }

    activate(params) {
        this.params = params;
        return this.http.get('projects/get/'+params.project+'/'+readCookie("userName")).then(response => {
            this.groups = response.content.groups;
            this.members = response.content.members;
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