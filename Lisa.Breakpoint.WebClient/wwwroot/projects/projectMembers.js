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
            var orgMembers = response.content;
            var orgMembersLength= 0;
            for(var key in orgMembers) {
                if(orgMembers.hasOwnProperty(key)){
                    orgMembersLength++;
                }
            }
            if (orgMembersLength > 0) {
                this.usersLeft = true;
                this.orgMembers = orgMembers;
            } else {
                this.usersLeft = false;
            }
        });
        return this.http.get('projects/get/'+params.project+'/'+readCookie("userName")).then(response => {
            this.members = response.content.members;
            var filteredGroups = this.filterGroups(response.content.groups, this.members);
            this.groups  = filteredGroups[0]; // list of groups
            this.disabled  = filteredGroups[1]; // groups for which you have no permission
            this.params  = params;
        });
    }

    addMember(member) {
        var userSel = document.getElementById("newMember");
        var member = userSel.options[userSel.selectedIndex].value;

        var roleSel = document.getElementById("newRole");
        var role = roleSel.options[roleSel.selectedIndex].value;

        var patch = {
            type: "add",
            member: member,
            role: role
        };

        this.http.patch('projects/'+this.params.project+'/members', patch).then(response => {
            window.location.reload();
        });
    }

    removeMember(member) {
        if (readCookie("userName") != member) {
            var patch = {
                type: "remove",
                member: member,
                role: ""
            };

            this.http.patch('projects/'+this.params.project+'/members', patch).then(response => {
                window.location.reload();
            });
        }
    }
    
    saveMember(member) {
        if (readCookie("userName") != member) {
            var sel = document.getElementById("role_"+member);
            var role = sel.options[sel.selectedIndex].value;

            var patch = {
                type: "update",
                member: member,
                role: role
            };

            this.http.patch('projects/'+this.params.project+'/members', patch).then(response => {
                window.location.reload();
            });
        }
    }

    filterGroups(groups, members) {
        var options = "";
        var disabled = [];
        
        groups.forEach(function(group, i) {
            if (group.name.indexOf("[n/a]") >= 1) {
                group.name = group.name.replace("[n/a]", "");
                var dGroup = {
                    name: group.name,
                    level: -1
                }
                disabled.push(dGroup);
                groups.splice(i,1);
            }
        });

        return [groups, disabled];
    }

    generateOptions(groups, disabled, members) {
        var options = "";
        var memberLength= 0;
        for(var key in members) {
            if(members.hasOwnProperty(key)){
                memberLength++;
            }
        }
        var groupLength= 0;
        for(var key in groups) {
            if(groups.hasOwnProperty(key)){
                groupLength++;
            }
        }
        for (var i = 0; i < memberLength; i++) {
            for (var j = 0; j < groupLength; j++) {
                var opt = document.createElement('option');
                opt.value = groups[j].name;
                opt.innerHTML = groups[j].name;

                options += opt;
            }
        }

        return options;
    }
}