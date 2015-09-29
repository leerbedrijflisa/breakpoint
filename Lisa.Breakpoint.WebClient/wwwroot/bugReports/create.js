import {Router} from 'aurelia-router';
import {HttpClient} from 'aurelia-http-client';

export class Create {
    static inject() {
        return [ Router ];
    }

    constructor(router) {
        this.router = router;
        this.http = new HttpClient().configure(x => {
            x.withBaseUrl('http://localhost:10791/');      
            x.withHeader('Content-Type', 'application/json')
        });
    }
    
    submit() {
        if (document.getElementById('personRadioButton').checked == true) {
            var data = {
                project: {
                    slug: this.project,
                    name: this.project
                },
                stepByStep: this.stepbystep,
                expectation: this.expectation,
                whatHappened: this.whathappened,
                reporter: {
                    userName: this.reporter,
                    fullName: this.reporter
                },
                status: "Open",
                priority: this.priority,
                assignedTo: "person",
                assignedToPerson: {
                    userName: this.assignedtoperson,
                    fullName: this.assignedtoperson
                }
            };
        } else {
            var data = {
                project: {
                    slug: this.project,
                    name: this.project
                },
                stepByStep: this.stepbystep,
                expectation: this.expectation,
                whatHappened: this.whathappened,
                reporter: {
                    userName: this.reporter,
                    fullName: this.reporter
                },
                status: "Open",
                priority: this.priority,
                assignedTo: "group",
                assignedToGroup: {
                    name: this.assignedToGroup,
                    members: [
                        {
                            userName: "developUser",
                            fullName: "developUser"
                        },
                        {
                            userName: "developUser222",
                            fullName: "developUser222"
                        }
                    ]
                }
            };
        }

        this.http.post('reports', data).then(response => {
            this.router.navigateToRoute("dashboard");
        });
    }
}