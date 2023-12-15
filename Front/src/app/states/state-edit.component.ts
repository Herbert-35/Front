import { Component, OnInit } from '@angular/core';
//import { HttpClient, HttpParams } from '@angular/common/http';
import { ActivatedRoute, Router } from '@angular/router';
import { FormGroup, FormBuilder, Validators, AbstractControl, AsyncValidatorFn } from '@angular/forms';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

//import { environment } from './../../environments/environment';
import { State } from './state';
import { BaseFormComponent } from '../base-form.component';
import { StateService } from './state.service';

@Component({
  selector: 'app-state-edit',
  templateUrl: './state-edit.component.html',
  styleUrls: ['./state-edit.component.scss']
})
export class StateEditComponent
  extends BaseFormComponent implements OnInit {


  // the view title
  title?: string;
  // the form model
  //form!: FormGroup;
  // the state object to edit or create
  state?: State;
  // the country object id, as fetched from the active route:
  // It's NULL when we're adding a new country,
  // and not NULL when we're editing an existing one.
  id?: number;
  // the countries array for the select
  states?: State[];
  constructor(
    private fb: FormBuilder,
    private activatedRoute: ActivatedRoute,
    private router: Router,
    private stateService: StateService)
  {
    super();
  }
  ngOnInit() {
    this.form = this.fb.group({
      name: ['',
        Validators.required,
        this.isDupeField("name"),
        Validators.minLength(2), Validators.maxLength(2),
        Validators.pattern(/^[a-zA-Z\s]*$/)
      ],
      restaurantName: ['',
        Validators.required,
        this.isDupeField("restaurantName")
      ],
      phoneNumber: ['',
        Validators.required,
        this.isDupeField("phoneNumber"),
        Validators.minLength(10), Validators.maxLength(10),
        Validators.pattern(/^[0-9]/)
      ],
      cuisine: ['',
        Validators.required
      ]
    });
    this.loadData();
  }
  loadData() {
    // retrieve the ID from the 'id' parameter
    var idParam = this.activatedRoute.snapshot.paramMap.get('id');
    this.id = idParam ? +idParam : 0;
    if (this.id) {
      // EDIT MODE
      // fetch the state from the server
      //var url = environment.baseUrl + "api/States/" + this.id;
      this.stateService.get(this.id)
        .subscribe(result => {
        this.state = result;
        this.title = "Edit - " + this.state.name;
        // update the form with the state value
        this.form.patchValue(this.state);
      }, error => console.error(error));
    }
    else {
      // ADD NEW MODE
      this.title = "Create a new State";
    }
  }
  onSubmit() {
    var state = (this.id) ? this.state : <State>{};
    if (state) {
      state.name = this.form.controls['name'].value;
      state.restaurantName = this.form.controls['restaurantName'].value;
      state.phoneNumber = +this.form.controls['phoneNumber'].value;
      state.cuisine = this.form.controls['cuisine'].value;
      if (this.id) {
        // EDIT mode
        //var url = environment.baseUrl + 'api/States/' + state.id;
        this.stateService
          .put(state)
          .subscribe(result => {
            console.log("Country " + state!.id + " has been updated.");
            // go back to states view
            this.router.navigate(['/states']);
          }, error => console.error(error));
      }
      else {
        // ADD NEW mode
        //var url = environment.baseUrl + 'api/States';
        this.stateService
          .post(state)
          .subscribe(result => {
            console.log("State " + result.id + " has been created.");
            // go back to countries view
            this.router.navigate(['/states']);
          }, error => console.error(error));
      }
    }
  }
  isDupeField(fieldName: string): AsyncValidatorFn {
    return (control: AbstractControl): Observable<{
      [key: string]: any;
    } | null> => {
      return this.stateService.isDupeField(
        this.id ?? 0,
        fieldName,
        control.value)
        .pipe(map(result => {
          return (result ? { isDupeField: true } : null);
        }));
    };
  }
}
