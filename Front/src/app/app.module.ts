import { HttpClientModule,HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthInterceptor } from './auth/auth.interceptor';
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { ReactiveFormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';

import { AppComponent } from './app.component';
import { HomeComponent } from './home/home.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { AngularMaterialModule } from './angular-material.module';

import { LocationsComponent } from './locations/locations.component';
import { StatesComponent } from './states/states.component';
import { LocationEditComponent } from './locations/location-edit.component';
import { StateEditComponent } from './states/state-edit.component';
import { BaseFormComponent } from './base-form.component';
import { LoginComponent } from './auth/login.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    NavMenuComponent,
    LocationsComponent,
    StatesComponent,
    LocationEditComponent,
    StateEditComponent,
    LoginComponent

  ],
  imports: [
    BrowserModule, ReactiveFormsModule, HttpClientModule,
    AppRoutingModule,
    BrowserAnimationsModule,
    AngularMaterialModule
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
