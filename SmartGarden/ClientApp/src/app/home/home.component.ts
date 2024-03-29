import { Component } from '@angular/core';
import { SignalRService } from '../core/services/signal-r.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(private signalRService: SignalRService) {
    this.signalRService.subscribeToGardenState();
  }
}
