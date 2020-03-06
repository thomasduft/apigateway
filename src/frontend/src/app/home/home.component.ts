import { Component, OnInit, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'tw-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class HomeComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}
