import { Component, OnInit } from '@angular/core';
import { UserIdentityService } from '../../services/user-identity.service';
import { UserService } from '../../services/user.service';
import { combineLatest } from 'rxjs';
import { User } from '../../models/user.model';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css',
  providers: [UserIdentityService]
})

export class NavbarComponent implements OnInit {
  currentUser: any;
  users: User[] = [];

  constructor(
    private userIdentity: UserIdentityService,
    public userService: UserService,

  ) { }

  ngOnInit() {
    combineLatest([
      this.userIdentity.currentUser
    ]).subscribe(([user]) => {
      this.currentUser = user || {};
      this.loadAllUsers();
    });
  }

  loadAllUsers(): void {
    this.userService.getAllUsers().subscribe(users => {
      this.users = users;
    });
  }

  logout(): void {
    this.userIdentity.signOut();
  }
}
