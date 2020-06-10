import {
  Component,
  HostBinding,
  OnInit,
  ViewEncapsulation,
  isDevMode
} from '@angular/core';

import { OAuthService, OAuthEvent } from 'angular-oauth2-oidc';
import { UserService } from './core';

@Component({
  selector: 'tw-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent implements OnInit {
  public get username(): string {
    return this.user.userName;
  }

  @HostBinding('class')
  public style = 'shell';

  public constructor(
    private user: UserService,
    private oauthService: OAuthService
  ) { }

  public ngOnInit(): void {
    this.configure();
  }

  private async configure() {
    this.oauthService.configure({
      clientId: 'frontend',
      issuer: 'https://localhost:5004',
      redirectUri: isDevMode()
        ? 'http://localhost:4200'
        : window.location.origin,
      responseType: 'code',
      scope: 'openid profile catalog orders.full_access time',
      loginUrl: 'https://localhost:5004/account/login',
      logoutUrl: 'https://localhost:5004/account/logout',
      requireHttps: false
    });
    this.oauthService.events.subscribe(async (e: OAuthEvent) => {
      // console.log(e);
      if (e.type === 'token_received' || e.type === 'token_refreshed') {
        this.user.setProperties(this.oauthService.getAccessToken());
      }

      if (e.type === 'discovery_document_loaded' && this.oauthService.hasValidAccessToken()) {
        // const userInfo = await this.oauthService.loadUserProfile();
        // console.log(userInfo);
        this.user.setProperties(this.oauthService.getAccessToken());
      }
    });
    this.oauthService.loadDiscoveryDocumentAndLogin({
      onTokenReceived: context => {
        this.user.setProperties(context.accessToken);
      }
    });
  }
}
