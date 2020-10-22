import {
  Component,
  HostBinding,
  OnInit,
  ViewEncapsulation
} from '@angular/core';

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
    private user: UserService
  ) { }

  public ngOnInit(): void {
  }
}
