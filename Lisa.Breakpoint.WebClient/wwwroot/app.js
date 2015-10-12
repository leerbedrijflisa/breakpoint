﻿import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class App {
    configureRouter(config, router) {
        config.title = 'Breakpoint';
        config.addPipelineStep('authorize', AuthorizeStep);
        config.map([
          { route: ['', 'user'],  name: 'login',   moduleId: 'users/user',   title:'User' },
          { route: 'user/logout', name: 'logout',  moduleId: 'users/logout', title:'Logout' },
          { route: 'user/group',  name: 'groups',  moduleId: 'users/group',  title:'Group' },

          { route: 'organization',    auth: true, name: 'organizations',          moduleId: 'organizations/organization',         title:'Organizations' },
          { route: 'organization/create',   auth: true, name: 'create-organization',    moduleId: 'organizations/createOrganization',    title:'New organization' },

          { route: ':organization',         auth: true, name: 'projects',       moduleId: 'projects/project',       title:'Projects' },
          { route: ':organization/create',  auth: true, name: 'create-project', moduleId: 'projects/createProject', title:'New project' },

          { route: ':organization/:project',            auth: true, name: 'reports',          moduleId: 'bugReports/dashboard',   title:'Reports' },
          { route: ':organization/:project/create',     auth: true, name: 'create-report',    moduleId: 'bugReports/create',      title:'New Report' },
          { route: ':organization/:project/edit/:id',   auth: true, name: 'edit-report',      moduleId: 'bugReports/edit',        title:'Edit Report' },
        ]);

        this.router = router;

        this.userName = "Logged in as: " + readCookie("userName");
        this.role     = "(" + readCookie("role") + ")";
    }
}


class AuthorizeStep {
    run(routingContext, next) {
        if (routingContext.nextInstructions.some(i => i.config.auth)) {
            var isLoggedIn = AuthorizeStep.isLoggedIn();
            if (!isLoggedIn) {
                //alert("Not Logged In!");
                return next.cancel();
            }
        }
        return next();
    }
 
    static isLoggedIn(): boolean {
        var auth_token = readCookie("userName");
        return (typeof auth_token !== "undefined" && auth_token !== null);
    }
}