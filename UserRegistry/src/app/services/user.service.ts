import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, map, } from 'rxjs';
import { environment } from '../environments/environment';
import { Status, User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})

export class UserService {

  baseApiUrl: string = environment.baseApi
  constructor(private http: HttpClient) { }

  getAllUsers(): Observable<User[]> {
    return this.http.get<User[]>(`${this.baseApiUrl}/users`);
  }

  getStatus(status: Status): string {
    const statuses = {
      [Status.Active]: 'Active',
      [Status.Blocked]: 'Blocked',
    };
    return statuses[status] || '';
  }

  deleteUsers(users: User[]): Observable<any> {
    const userIds = users.map(user => user.userId);
    return this.http.request('delete', `${this.baseApiUrl}/delete-users`, { body: userIds });
  }

  setAccountStatus(userId: number, status: Status): Observable<any> {
    const body = {
      status: status,
    };
    return this.http.put(`${this.baseApiUrl}/${userId}/status`, body);
  }

  setUserSelection(userId: number, isSelected: boolean): Observable<any> {
    const url = `${this.baseApiUrl}/${userId}/selection`;
    const body = { isSelected };
    return this.http.put(url, body);
  }
}
