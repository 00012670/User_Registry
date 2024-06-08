import { Injectable } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import ValidateForm from '../helpers/validateForm';
import { NgToastService } from 'ng-angular-popup';
import { Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class FormValidationService {

  userId!: number;
  submitted = false;

  constructor(
    private toast: NgToastService,
    private formBuilder: FormBuilder,
  ) { }

  SignupValidator() {
    return this.formBuilder.group({
      username: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required]
    });
  }

  LoginValidator() {
    return this.formBuilder.group({
      username: ['', Validators.required],
      password: ['', Validators.required]
    });
  }

  validateForm(form: FormGroup) {
    if (form.valid) {
      return true;
    } else {
      ValidateForm.validateAllFormFileds(form);
      this.toast.error({ detail: "ERROR", summary: "Your form is invalid", duration: 5000 });
      return false;
    }
  }


  handleSuccess(message: string, router: Router, route: string): void {
    router.navigate([route]);
    this.toast.success({ detail: 'SUCCESS', summary: message, duration: 4000 });
  }

  handleError(error: any, message: string): void {
    this.toast.error({ detail: 'ERROR', summary: message, duration: 4000 });
  }
}
