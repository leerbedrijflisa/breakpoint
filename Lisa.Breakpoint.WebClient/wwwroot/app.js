﻿import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class App {
    configureRouter(config, router) {
        config.title = 'Breakpoint';
        config.map([
          { route: ['', 'user'], name: 'user', moduleId: 'users/user', title:'User' },

          { route: ['', 'organization'],    name: 'organizations',          moduleId: 'organizations/organization',         title:'Organizations' },
          { route: 'organization/create',   name: 'create-organization',    moduleId: 'organizations/createOrganization',    title:'New organization' },

          { route: ':organization',         name: 'projects',       moduleId: 'projects/project',       title:'Projects' },
          { route: ':organization/create',  name: 'create-project', moduleId: 'projects/createProject', title:'New project' },

          { route: ':organization/:project',            name: 'reports',          moduleId: 'bugReports/dashboard',   title:'Reports' },
          { route: ':organization/:project/create',     name: 'create-report',    moduleId: 'bugReports/create',      title:'New Report' },
          { route: ':organization/:project/:id/edit',   name: 'edit-report',      moduleId: 'bugReports/edit',        title:'Edit Report' },
        ]);

        this.router = router;

        //fake user login
        setCookie("userName", "baseenhoorn", 2);
        this.userName = readCookie("userName");
    }
}