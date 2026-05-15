import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';
import { NgSelectModule } from '@ng-select/ng-select';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { TasksFormComponent } from './features/tasks/tasks-form/tasks-form.component';
import { TasksComponent } from './features/tasks/tasks/tasks.component';
import { ColonyStateComponent } from './features/colony-state/colony-state.component';
import { AlertsComponent } from './features/alerts/alerts.component';
import { CrewComponent } from './features/crew/crew.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    TasksComponent,
    TasksFormComponent,
    ColonyStateComponent,
    AlertsComponent,
    CrewComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule,
    FormsModule,
    NgSelectModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
