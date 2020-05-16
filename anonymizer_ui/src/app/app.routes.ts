import {Routes, RouterModule} from '@angular/router';
import {ModuleWithProviders} from '@angular/core';
import { JobDetailsComponent } from './demo/view/jobdetails.component';
import { LogComponent } from './demo/view/logs.component';
import { CreateSqlJobComponent } from './demo/view/createsqljob.component';

export const routes: Routes = [
    {path: 'jobdetails', component: JobDetailsComponent},
    {path: 'logs', component: LogComponent},
    {path: '', component: LogComponent},
    {path: 'createjob', component: CreateSqlJobComponent},
    { path: '**', redirectTo: '/' }
];

export const AppRoutes: ModuleWithProviders = RouterModule.forRoot(routes, {scrollPositionRestoration: 'enabled'});
