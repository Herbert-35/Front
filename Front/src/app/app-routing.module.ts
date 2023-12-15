import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { LocationsComponent } from './locations/locations.component';
import { LocationEditComponent } from './locations/location-edit.component';
import { StatesComponent } from './states/states.component';
import { StateEditComponent } from './states/state-edit.component';
import { LoginComponent } from './auth/login.component';
import { AuthGuard } from './auth/auth.guard';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'locations', component: LocationsComponent },
  { path: 'location/:id', component: LocationEditComponent, canActivate: [AuthGuard] },
  { path: 'location', component: LocationEditComponent, canActivate: [AuthGuard] },
  { path: 'states', component: StatesComponent },
  { path: 'state/:id', component: StateEditComponent, canActivate: [AuthGuard] },
  { path: 'state', component: StateEditComponent, canActivate: [AuthGuard] },
  { path: 'login', component: LoginComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
