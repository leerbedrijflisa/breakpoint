﻿import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class App {
    configureRouter(config, router){
        config.title = 'Breakpoint';
        config.map([

          { route: ['','report'], name: 'report', moduleId: 'report', nav: true, title:'report' },
          { route: 'dashboard', name: 'dashboard', moduleId: 'dashboard', nav: true, title:'dashboard' },
          { route: ['', 'user'], name: 'user', moduleId: 'user', nav: true, title:'User' },
          { route: 'project', name: 'project', moduleId: 'project', nav: true, title:'project' },
          { route: 'project/create', name: 'create-project', moduleId: 'createProject', nav: true, title:'Create project' },
          { route: 'bug-reports/create', name: 'createBugReport', moduleId: 'bugReports/create', nav: true, title:'New Report' },
          { route: 'edit', name: 'edit', moduleId: 'edit', nav: true, title:'Edit Report' }

        ]);

        this.router = router;
    }
}