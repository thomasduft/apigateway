import { Observable } from 'rxjs';

import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { HttpWrapperService } from '../core/services';
import { Time } from './models';

@Component({
  selector: 'tw-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class HomeComponent implements OnInit {
  public time: Time

  constructor(
    private http: HttpWrapperService
  ) { }

  ngOnInit(): void {
    this.http.get<Time>('time-api/time').subscribe((t: Time) => {
      this.time = t;
    });
  }
}
