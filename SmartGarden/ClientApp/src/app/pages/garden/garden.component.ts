import { Component } from '@angular/core';
import { filter } from 'rxjs';
import { GardenService } from 'src/app/core/services/garden.service';

@Component({
  selector: 'app-garden',
  templateUrl: './garden.component.html',
  styleUrl: './garden.component.scss',
})
export class GardenComponent {
  public gardenState$ = this.gardenSvc.gardenState$.pipe(filter((s) => !!s));
  public loadingPump: boolean = false;
  public loadingWindows: boolean = false;

  constructor(private gardenSvc: GardenService) {}

  public openPump() {
    this.loadingPump = true;
    this.gardenSvc.openPump().subscribe((res) => {
      this.loadingPump = false;
    });
  }

  public openWindows() {
    this.loadingWindows = true;
    this.gardenSvc.openWindows().subscribe((res) => {
      this.loadingWindows = false;
    });
  }
}
