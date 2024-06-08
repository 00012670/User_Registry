import { CommonModule } from '@angular/common';
import { Component, NgModule } from '@angular/core';
import { FormGroup, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { UserIdentityService } from '../../../services/user-identity.service';
import { FormValidationService } from '../../../services/form-validation.service';

@Component({
  selector: 'app-signup',
  standalone: true,
  imports: [ReactiveFormsModule, CommonModule],
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
  providers: [UserIdentityService]
})

export class SignupComponent {
  type: string = 'password';
  isText: boolean = false;
  eyeIcon: string = 'fa-eye-slash';
  signUpForm!: FormGroup;
  isDarkMode!: boolean;

  constructor(
    private identityService: UserIdentityService,
    private router: Router,
    private formValidation: FormValidationService,
  ) {
    this.signUpForm = this.formValidation.SignupValidator();
  }

  ngOnInit(): void {
  }

  onSignup() {
    if (this.formValidation.validateForm(this.signUpForm)) {
      this.identityService.signUp(this.signUpForm.value).subscribe(
        (response: any) => {
          console.log('Signup response:', response);
          const token = response.token;
          const decodedToken = this.identityService.decodeToken(token);
          this.identityService.currentUser.next(decodedToken);
          this.formValidation.handleSuccess("Signup successfully", this.router, 'dashboard');
        },
        (error: { message: any; }) => {
          this.formValidation.handleError(error, error.message);
        }
      );
    }
  }

  hideShowPass() {
    this.isText = !this.isText;
    this.eyeIcon = this.isText ? 'fa-eye' : 'fa-eye-slash';
    this.type = this.isText ? 'text' : 'password';
  }
}
