import { Component, OnInit, HostBinding, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

import { OAuthService } from 'angular-oauth2-oidc';

import { UserService } from '../core';

@Component({
  selector: 'tw-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class SidebarComponent implements OnInit {

  @HostBinding('class')
  public classlist = 'sidebar';

  public get userName(): string {
    return this.user.userName;
  }

  public constructor(
    private router: Router,
    private user: UserService,
    private oauthService: OAuthService
  ) { }

  public ngOnInit(): void {
  }

  public logout(): void {
    this.oauthService.logOut();
    this.user.reset();
    this.router.navigate(['/']);
  }
}
