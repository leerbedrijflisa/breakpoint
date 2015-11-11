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
        this.loggedInUser = readCookie("userName");

        return Promise.all([
            this.http.get('organizations/members/new/'+params.organization+'/'+params.project).then(response => {
                var orgMembers = response.content;
                var orgMembersLength = 0;
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
            }),
            this.http.get('projects/'+params.organization+'/'+params.project+'/'+readCookie("userName")).then(response => {
                this.members = this.filterMembers(response.content.members);
                this.groups  = response.content.groups;
            })
        ]);
    }

    addMember(member) {
        var member = getSelectValue("newMember");
        var role = getSelectValue("newRole");

        var patch = {
            sender: readCookie("userName"),
            type: "add",
            member: member,
            role: role
        };

        this.http.patch('projects/'+this.params.organization+'/'+this.params.project+'/members', patch).then(response => {
            window.location.reload();
        });
    }

    removeMember(member) {
        var role = getSelectValue("role_"+member);

        if (readCookie("userName") != member) {
            var patch = {
                sender: readCookie("userName"),
                type: "remove",
                member: member,
                role: role
            };

            this.http.patch('projects/'+this.params.organization+'/'+this.params.project+'/members', patch).then(response => {
                window.location.reload();
            });
        }
    }
    
    saveMember(member) {
        if (readCookie("userName") != member) {
            var role = getSelectValue("role_"+member);

            var patch = { 
                sender: readCookie("userName"),
                type: "update",
                member: member,
                role: role
            };

            this.http.patch('projects/'+this.params.organization+'/'+this.params.project+'/members', patch).then(response => {
                window.location.reload();
            });
        }
    }

    filterMembers(members) {
        var membersLength = 0;
        var i,
            loggedInUserRole,
            loggedInUserRoleLevel,
            memberRoleLevel;

        for(var key in members) {
            if(members.hasOwnProperty(key)){
                membersLength++;
            }
        }

        for (i = 0; i < membersLength; i++) {
            if (members[i].userName == readCookie("userName")) {
                loggedInUserRole = members[i].role;
                break;
            }
        }

        switch (loggedInUserRole) {
            case "manager":
                loggedInUserRoleLevel = 2;
                break;
            case "developer":
                loggedInUserRoleLevel = 1;
                break;
            case "tester":
                loggedInUserRoleLevel = 0;
                break;
            default:
                loggedInUserRoleLevel = 0;
                break;
        }

        for (i = 0; i < membersLength; i++) {
            switch (members[i].role) {
                case "manager":
                    memberRoleLevel = 2;
                    break;
                case "developer":
                    memberRoleLevel = 1;
                    break;
                case "tester":
                    memberRoleLevel = 0;
                    break;
                default:
                    memberRoleLevel = 0;
                    break;
            }

            if (memberRoleLevel > loggedInUserRoleLevel) {
                this.canEditMember[i] = false;
            } else {
                this.canEditMember[i] = true;
            }            
        }

        return members;
    }
}