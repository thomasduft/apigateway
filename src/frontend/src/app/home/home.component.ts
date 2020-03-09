import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';

import { Component, OnInit, ViewEncapsulation, OnDestroy } from '@angular/core';

import { environment } from '../../environments/environment';
import { HttpWrapperService } from '../core/services';
import { Time } from './models';

@Component({
  selector: 'tw-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.less'],
  encapsulation: ViewEncapsulation.None
})
export class HomeComponent implements OnInit, OnDestroy {
  private hubConnection: HubConnection;

  public time: Time;

  constructor(
    private http: HttpWrapperService
  ) { }

  ngOnInit(): void {
    this.http.get<Time>('time-api/time').subscribe((t: Time) => {
      this.time = t;
    });

    this.init();
  }

  ngOnDestroy(): void {
    this.hubConnection.stop();
  }

  public init(): void {
    this.createConnection();
    this.registerOnServerEvents();
    this.startConnection();
  }

  private createConnection() {
    // /time-ws/timeHub
    // https://localhost:5000/timeHub
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(`${environment.domain}/time-ws/timeHub`)
      .build();
  }

  private registerOnServerEvents(): void {
    this.hubConnection.on('time', (data: any) => {
      console.log('time message arrived', data);

      this.time = data;
    });
    this.hubConnection.onclose((error => {
      // TODO: some indication that connection got lost!
      console.log(error);
    }));
  }

  private startConnection() {
    this.hubConnection.start()
      .then(() => {
        console.log('ws connection started...');
      })
      .catch(error => console.log(error));
  }
}
