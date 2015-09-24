﻿import {inject} from 'aurelia-framework';
import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class App {
    configureRouter(config, router){
        config.title = 'Aurelia';
        config.map([
          { route: ['','report'], name: 'report', moduleId: 'report', nav: true, title:'report' },
          { route: 'dashboard', name: 'dashboard', moduleId: 'dashboard', nav: true, title:'dashboard' },
          { route: 'project', name: 'project', moduleId: 'project', nav: true, title:'project' },
          { route: 'project/create', name: 'create-project', moduleId: 'createProject', nav: true, title:'Create project' }
        ]);

        this.router = router;
    }
}