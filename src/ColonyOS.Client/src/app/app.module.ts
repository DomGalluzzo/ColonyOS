import { HttpClientModule } from '@angular/common/http';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { TasksFormComponent } from './features/tasks/tasks-form/tasks-form.component';
import { TasksComponent } from './features/tasks/tasks/tasks.component';
import { ReactiveFormsModule } from '@angular/forms';
import { ColonyStateComponent } from './features/colony-state/colony-state.component';
import { AlertsComponent } from './features/alerts/alerts.component';

@NgModule({
  declarations: [
    AppComponent,
    DashboardComponent,
    TasksComponent,
    TasksFormComponent,
    ColonyStateComponent,
    AlertsComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    ReactiveFormsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
