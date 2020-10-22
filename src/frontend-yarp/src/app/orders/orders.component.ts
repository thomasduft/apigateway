import { Observable } from 'rxjs';

import { Component, OnInit, ViewEncapsulation } from '@angular/core';

import { HttpWrapperService } from '../core/services';
import { Order } from './models';

@Component({
  selector: 'tw-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class OrdersComponent implements OnInit {
  public orders$: Observable<Array<Order>>;

  constructor(
    private http: HttpWrapperService
  ) { }

  ngOnInit(): void {
    this.orders$ = this.http.get<Array<Order>>('order-api/orders');
  }
}
