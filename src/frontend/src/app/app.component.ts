import { Component, HostBinding, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'tw-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent {
  title = 'frontend';

  @HostBinding('class')
  public style = 'shell';
}
