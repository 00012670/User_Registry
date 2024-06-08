import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from '../environments/environment';
import { Router } from '@angular/router';
import { BehaviorSubject, catchError, tap, throwError } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class UserIdentityService {
  currentUser: BehaviorSubject<any> = new BehaviorSubject(null);
  baseApi: string = environment.baseApi

  constructor(
    private http: HttpClient,
    private router: Router,
    private jwtHelper: JwtHelperService) {
    const storedToken = localStorage.getItem('token');
    if (storedToken) {
      const user = this.decodeToken(storedToken);
      if (user && user.role) {
        this.currentUser.next(user);
      }
    }
  }

  errorHandler(error: HttpErrorResponse) {
    return throwError(error.error || 'Server Error');
  }

  signUp(userObj: any) {
    return this.http.post<any>(`${this.baseApi}/Identity/register`, userObj)
      .pipe(catchError(this.errorHandler),
        tap((response) => {
          this.currentUser.next(response);
          localStorage.setItem('token', JSON.stringify(response));
        }));
  }

  signIn(loginObj: any) {
    return this.http.post<any>(`${this.baseApi}/Identity/login`, loginObj)
      .pipe(catchError(this.errorHandler),
        tap((response) => {
          this.currentUser.next(response);
          localStorage.setItem('token', JSON.stringify(response));
        }));
  }

  signOut() {
    localStorage.clear();
    this.currentUser.next(null);
    this.router.navigate(['login'])
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token')
  }

  storeToken(tokenValue: string) {
    localStorage.setItem('token', tokenValue)
  }

  getToken() {
    return localStorage.getItem('token')
  }

  decodeToken(token: string) {
    const decodedToken = this.jwtHelper.decodeToken(token);
    console.log(decodedToken);
    // Check if the 'nameid' field exists in the decoded token
    if ('nameid' in decodedToken) {
      return decodedToken.nameid;
    } else {
      console.log('nameid field does not exist in the decoded token');
      return null;
    }

  }

  getUserId(): number | null {
    // Check if currentUser.value exists and if it has a 'nameid' property
    if (this.currentUser.value && 'nameid' in this.currentUser.value) {
      return Number(this.currentUser.value.nameid);
    } else {
      console.log('currentUser.value is null or does not have a nameid property');
      return null;
    }
  }
}
