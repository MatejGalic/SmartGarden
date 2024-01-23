import { Component } from '@angular/core';
import { GardenService } from 'src/app/core/services/garden.service';

@Component({
  selector: 'app-garden',
  templateUrl: './garden.component.html',
  styleUrl: './garden.component.scss',
})
export class GardenComponent {
  public gardenState$ = this.gardenSvc.gardenState$;
  public loadingPump: boolean = false;
  public loadingWindows: boolean = false;

  constructor(private gardenSvc: GardenService) {}

  public togglePump(state: boolean) {
    this.loadingPump = true;
    this.gardenSvc.togglePump({ shouldItemOpen: !state }).subscribe((res) => {
      this.loadingPump = false;
    });
  }

  public toggleWindows(state: boolean) {
    this.loadingWindows = true;
    this.gardenSvc
      .toggleWindows({ shouldItemOpen: !state })
      .subscribe((res) => {
        this.loadingWindows = false;
      });
  }
}
