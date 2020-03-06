import { Component, OnInit, HostBinding, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'tw-sidebar',
  templateUrl: './sidebar.component.html',
  styleUrls: ['./sidebar.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class SidebarComponent implements OnInit {

  @HostBinding('class')
  public classlist = 'sidebar';

  constructor() { }

  ngOnInit(): void {
  }

}
