import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { AbstractControl, FormBuilder, FormControl, FormGroup, FormsModule, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { ToastrService } from 'ngx-toastr';
import { JsonPipe, NgIf } from '@angular/common';
import { TextInputComponent } from "../_forms/text-input/text-input.component";
import { DatePickerComponent } from "../_forms/date-picker/date-picker.component";
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [ReactiveFormsModule, TextInputComponent, DatePickerComponent],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent implements OnInit{
model : any ={};
maxDate = new Date();
registerForm : FormGroup = new FormGroup({});
private accountService = inject(AccountService);
private toastr = inject(ToastrService);
private fb = inject(FormBuilder);
//@Input() usersFromHomeComponent : any;
@Output() cancelRegister = new EventEmitter();
private router = inject (Router);
validationErrors: string[] | undefined;


ngOnInit(): void {
  this.initializeFrom();
  this.maxDate.setFullYear(this.maxDate.getFullYear() - 18)
}

// initializeFrom(){
//   this.registerForm = new FormGroup({
//     username: new FormControl('', Validators.required),
//     gender: new FormControl('male'),
//     knownAs: new FormControl('', Validators.required),
//     dateOfBirth: new FormControl('', Validators.required),
//     city: new FormControl('', Validators.required),
//     country: new FormControl('', Validators.required),
//     password: new FormControl('', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]),
//     confirmPassword: new FormControl('', [Validators.required, this.matchValues('password')]),
//     });
//   //add a condition if p.w values change then call update value validity method
//   this.registerForm.controls['password'].valueChanges.subscribe({
//     next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
//   })  
// }

initializeFrom(){
  this.registerForm = this.fb.group({
    username: ['', Validators.required],
    gender: ['male'],
    knownAs: ['', Validators.required],
    dateOfBirth: ['', Validators.required],
    city: ['', Validators.required],
    country:['', Validators.required],
    password: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(8)]],
    confirmPassword: ['', [Validators.required, this.matchValues('password')]],
    });
  //add a condition if p.w values change then call update value validity method
  this.registerForm.controls['password'].valueChanges.subscribe({
    next: () => this.registerForm.controls['confirmPassword'].updateValueAndValidity()
  })  
}



//create a function to compare p.w n confirm p.w
matchValues(matchTo: string): ValidatorFn{
  return(control : AbstractControl) =>{
    return control.value === control.parent?.get(matchTo)?.value? null : { isMatching : true}
  }
}

//add methods
register(){  
  const dob = this.getDateOnly(this.registerForm.get('dateOfBirth')?.value);
  this.registerForm.patchValue({dateOfBirth: dob});

  this.accountService.register(this.registerForm.value).subscribe({
    next : _ =>this.router.navigateByUrl('/members'),
    error : error => this.validationErrors = error
  })
}

cancel(){
  this.cancelRegister.emit(false);
}

private getDateOnly(dob: string | undefined){
  if(!dob) return;

  return new Date(dob).toISOString().slice(0,10);
}
}
