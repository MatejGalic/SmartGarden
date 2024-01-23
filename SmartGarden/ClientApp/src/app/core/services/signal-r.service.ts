// signalr.service.ts
import { Inject, Injectable } from '@angular/core';
import { HubConnectionBuilder } from '@microsoft/signalr';
import { GardenParameters } from '../models/dtos/gardenParameters';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class SignalRService {
  private hubConnection: signalR.HubConnection;

  private _gardenState = new Subject<GardenParameters>();
  public gardenState$ = this._gardenState.asObservable();

  constructor(@Inject('BASE_URL') baseUrl: string) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(baseUrl + 'hub')
      .build();
  }

  startConnection() {
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch((err) => console.log('Error while starting connection: ' + err));
  }

  // Additional methods for handling hub events can be added here

  subscribeToGardenState() {
    this.hubConnection.on('GardenParameters', (state: GardenParameters) => {
      console.log('Received message from server: ', state);
      this._gardenState.next(state);
    });
  }
}
