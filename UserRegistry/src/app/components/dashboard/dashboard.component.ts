import { Component, OnInit } from '@angular/core';
import { UserIdentityService } from '../../services/user-identity.service';
import { UserService } from '../../services/user.service';
import { UserOperationsService } from '../../services/user-operaton.service';
import { Status, User } from '../../models/user.model';
import { FormValidationService } from '../../services/form-validation.service';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { combineLatest } from 'rxjs';
import { NavbarComponent } from '../navbar/navbar.component';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [FormsModule, CommonModule, RouterModule, ReactiveFormsModule, NavbarComponent],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css',
  providers: [UserIdentityService, UserService, UserOperationsService, FormValidationService]
})

export class DashboardComponent implements OnInit {
  userId: number | null = null;
  users: User[] = [];
  currentUser: any;
  selectedUsers: User[] = [];
  searchText: any;
  isDarkMode!: boolean;

  constructor(
    public userIdentity: UserIdentityService,
    public userService: UserService,
    private userOperations: UserOperationsService,
    private formValidation: FormValidationService,
  ) { }

  ngOnInit(): void {
    this.loadAllUsers();
    this.subscribeToCurrentUser();
  }

  private subscribeToCurrentUser(): void {
    combineLatest([
      this.userIdentity.currentUser,
    ]).subscribe(([user]) => {
      this.currentUser = user;
      console.log(this.currentUser);
      this.setSelectedUsers();
    });
  }

  setSelectedUsers(): void {
    if (this.currentUser && 'nameid' in this.currentUser) {
      this.selectedUsers = this.userOperations.getSelectedUsersState();
      this.selectedUsers.forEach(selectedUser => {
        const user = this.users.find(user => user.userId === selectedUser.userId);
        if (user) {
          user.isSelected = true;
        }
      });
    } else {
      console.log('currentUser is null or does not have a nameid property');
    }
  }

  loadAllUsers(): void {
    this.userService.getAllUsers().subscribe(users => {
      this.users = users;
    });
  }

  toggleSelection(user: User): void {
    this.userOperations.toggleUserSelection(user);
    this.selectedUsers = this.userOperations.getSelectedUsers(this.users);
    this.userOperations.storeSelectedUsersState(this.selectedUsers);
  }

  saveChanges(): void {
    this.userOperations.updateUsersSelection(this.selectedUsers, true).subscribe();
    this.selectedUsers = [];
  }

  deleteSelectedUsers(): void {
    this.userOperations.deleteUsers(this.selectedUsers).subscribe(() => {
      this.users = this.users.filter(user => !user.isSelected);
      this.selectedUsers = [];
    });
  }

  blockSelectedUsers(): void {
    if (!this.hasSelectedUsers()) return;
    this.updateUserStatus(Status.Blocked);
  }

  unblockSelectedUsers(): void {
    if (!this.hasSelectedUsers()) return;
    this.updateUserStatus(Status.Active);
  }

  private updateUserStatus(status: Status): void {
    const usersToUpdate = this.selectedUsers.length > 0 ? this.selectedUsers : this.users;
    this.userOperations.updateUsersStatus(usersToUpdate, status).subscribe(() => {
      this.updateLocalUsersStatus(status);
      this.selectedUsers = [];
    });
  }

  private hasSelectedUsers(): boolean {
    if (this.selectedUsers.length === 0) {
      this.formValidation.handleError(null, ('No users selected'));
      return false;
    }
    return true;
  }

  private updateLocalUsersStatus(status: Status): void {
    this.selectedUsers.forEach(user => {
      user.status = status;
      user.isSelected = false;
    });
  }
}
