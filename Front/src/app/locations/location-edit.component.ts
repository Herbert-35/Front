import { Component, OnInit } from '@angular/core';
//import { HttpClient,HttpParams } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FormGroup, FormControl, Validators, AbstractControl, AsyncValidatorFn
} from '@angular/forms';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

import { environment } from './../../environments/environment';
import { Location } from './location';
import { State } from './../states/state';
import { BaseFormComponent } from '../base-form.component';
import { LocationService } from './location.service';
import { ApiResult } from '../base.service';

@Component({
  selector: 'app-location-edit',
  templateUrl: './location-edit.component.html',
  styleUrl: './location-edit.component.scss'
})
export class LocationEditComponent
extends BaseFormComponent  implements OnInit {

  // the view title
  title?: string;

  // the form model
  //form!: FormGroup;
  // the location object to edit or create
  location?: Location;

  // the location object id, as fetched from the active route:
  // It's NULL when we're adding a new location,
  // and not NULL when we're editing an existing one.
  id?: number;

  // the states array for the select
  states?: State[];

  constructor(
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private locationService: LocationService)
  {
    super();
  }
  ngOnInit() {
    this.form = new FormGroup({
      name: new FormControl('',
        [Validators.required, Validators.pattern(/^[a-zA-Z\s]*$/)]),
      zipCode: new FormControl('',
        [Validators.required, Validators.minLength(5), Validators.maxLength(5),
          Validators.pattern(/^[0-9]/)]),
      streetAddress: new FormControl('', Validators.required),
      stateId: new FormControl('', Validators.required)
    },null,this.isDupeLocation());
    this.loadData();
  }

  isDupeLocation(): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{ [key: string]: any } |
      null> => {

      var location = <Location>{};
      location.id = (this.id) ? this.id : 0;
      location.name = this.form.controls['name'].value;
      location.zipCode = +this.form.controls['zipCode'].value;
      location.streetAddress = this.form.controls['streetAddress'].value;
      location.stateId = +this.form.controls['stateId'].value;

      //var url = environment.baseUrl + 'api/locations/IsDupeLocation';
      return this.locationService.isDupeLocation(location).pipe(map(result => {
        return (result ? { isDupeLocation: true } : null);
      }));
    }
  }


  loadData() {

    // load states
    this.loadStates();

    // retrieve the ID from the 'id' parameter
    var idParam = this.activatedRoute.snapshot.paramMap.get('id');
    this.id = idParam ? +idParam : 0;
    if (this.id) {
      // EDIT MODE

      // fetch the location from the server
      //var url = environment.baseUrl + 'api/Locations/' + this.id;
      this.locationService.get(this.id).subscribe(result => {
        this.location = result;
        this.title = "Edit - " + this.location.name;

        // update the form with the location value
        this.form.patchValue(this.location);
      }, error => console.error(error));
    }
    else {
      // ADD NEW MODE

      this.title = "Create a new Location";
    }
  }

  loadStates() {
    // fetch all the states from the server
    //var url = environment.baseUrl + 'api/States';
    this.locationService.getStates(
      0,
      9999,
      "name",
      "asc",
      null,
      null,
    ).subscribe(result => {
      this.states = result.data;
    }, error => console.error(error));
  }

  onSubmit() {
    var location = (this.id) ? this.location : <Location>{};
    if (location) {
      location.name = this.form.controls['name'].value;
      location.zipCode = +this.form.controls['zipCode'].value;
      location.streetAddress = this.form.controls['streetAddress'].value;
      location.stateId = +this.form.controls['stateId'].value;

      if (this.id) {
        // EDIT mode
        this.locationService
          .put(location)
          .subscribe(result => {
            console.log("Location " + location!.id + " has been updated.");

            // go back to locations view
            this.router.navigate(['/locations']);
          }, error => console.error(error));
      }
      else {
        // ADD NEW mode
        this.locationService
          .post(location)
          .subscribe(result => {
            console.log("Location " + result.id + " has been created.");

            // go back to locations view
            this.router.navigate(['/locations']);
          }, error => console.error(error));
      }
    }
  }
}
