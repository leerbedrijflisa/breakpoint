﻿import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class App {
    configureRouter(config, router){
        config.title = 'Aurelia';
        config.map([
          { route: ['','report'], name: 'report', moduleId: 'report', nav: true, title:'Report' },
          { route: 'dashboard', name: 'dashboard', moduleId: 'dashboard', nav: true, title:'Dashboard' },
          { route: 'insert', name: 'insert', moduleId: 'insert', nav: true, title:'New Report' },
          { route: 'edit', name: 'edit', moduleId: 'edit', nav: true, title:'Edit Report' },
          
          { route: 'dashboard/projects', name: 'projects', moduleId: 'projects', nav: true, title:'Projects' },
          { route: 'dashboard/projects/insert', name: 'insertProject', moduleId: 'insertProject', nav: true, title:'Insert Project' },
        ]);

        this.router = router;
    }
}