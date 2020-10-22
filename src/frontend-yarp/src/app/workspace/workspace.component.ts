import { Component, OnInit, HostBinding, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'tw-workspace',
  templateUrl: './workspace.component.html',
  styleUrls: ['./workspace.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class WorkspaceComponent implements OnInit {
  @HostBinding('class')
  public style = 'workspace';

  constructor() { }

  ngOnInit(): void {
  }

}
