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

        // (in the API) this gets 2 member lists; Organization- and Projectmembers.
        // then it compares those and returns only those not already in the project
        // so you can only add new members to the project
        this.http.get('organizations/members/new/'+params.organization+'/'+params.project).then(response => {
            this.orgMembers = response.content;
        });
        return this.http.get('projects/get/'+params.project+'/'+readCookie("userName")).then(response => {
            this.groups = response.content.groups;
            this.members = response.content.members;
            this.params = params;
        });
    }

    addMember(member) {
        var userSel = document.getElementById("newMember");
        var userName = userSel.options[userSel.selectedIndex].value;

        var roleSel = document.getElementById("newRole");
        var role = roleSel.options[roleSel.selectedIndex].value;

        var patch = {
            type: "add",
            key: "username",
            value: userName
        };

        var patch2 = {
            type: "update",
            key: "role",
            value: role,
            where: "username",
            whereVal: userName
        };

        //this.http.patch('projects/member/'+this.params.project, patch).then(response => {
        console.log(patch);
        console.log(patch2);
        //});

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