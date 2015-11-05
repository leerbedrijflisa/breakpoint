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
        this.canEditMember = [];
        this.isLoggedInUser = [];

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
        return this.http.get('projects/'+params.organization+'/'+params.project+'/'+readCookie("userName")).then(response => {
            console.log(response.content.members);
            this.members = this.filterMembers(response.content.members);
            var filteredGroups = this.filterGroups(response.content.groups, this.members);
            this.groups  = filteredGroups[0];
            this.disabled  = filteredGroups[1];
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

        this.http.patch('projects/'+this.params.organization+'/'+this.params.project+'/members', patch).then(response => {
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

            this.http.patch('projects/'+this.params.organization+'/'+this.params.project+'/members', patch).then(response => {
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

            this.http.patch('projects/'+this.params.organization+'/'+this.params.project+'/members', patch).then(response => {
                window.location.reload();
            });
        }
    }

    // returns the groups an array
    // arr[0] = Groups lower than and equal to the logged-in user's role
    // arr[1] = Groups higher than the logged-in user's role
    filterGroups(groups, members) {
        var options = "";
        var disabled = [];
        
        groups.forEach(function(group, i) {
            if (group.name.indexOf("[n/a]") != -1) {
                group.name = group.name.replace("[n/a]", "");
                var dGroup = {
                    name: group.name,
                    level: -1
                }
                disabled.push(dGroup);
                groups.splice(i,1);
            }
        });

        groups.forEach(function(group, i) {
            if (group.name.indexOf("[n/a]") != -1) {
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

    filterMembers(members) {
        var membersLength= 0;
        for(var key in members) {
            if(members.hasOwnProperty(key)){
                membersLength++;
            }
        }

        var i;
        var loggedInUserRole;
        var loggedInUserRoleLevel;
        for (i = 0; i < membersLength; i++) {
            if (members[i].userName == readCookie("userName")) {
                loggedInUserRole = members[i].role;
                this.isLoggedInUser[i] = true;
            } else {
                this.isLoggedInUser[i] = false;
            }
        }


        switch (loggedInUserRole) {
            case "manager":
                loggedInUserRoleLevel = 0;
                break;
            case "developer":
                loggedInUserRoleLevel = 1;
                break;
            case "tester":
                loggedInUserRoleLevel = 2;
                break;
            default:
                loggedInUserRoleLevel = 2;
                break;
        }

        var memberRoleLevel
        for (i = 0; i < membersLength; i++) {
            switch (members[i].role) {
                case "manager":
                    memberRoleLevel = 0;
                    break;
                case "developer":
                    memberRoleLevel = 1;
                    break;
                case "tester":
                    memberRoleLevel = 2;
                    break;
                default:
                    memberRoleLevel = 2;
                    break;
            }

            if (memberRoleLevel < loggedInUserRoleLevel) {
                this.canEditMember[i] = false;
            } else {
                this.canEditMember[i] = true;
            }            
        }

        return members;
    }
}