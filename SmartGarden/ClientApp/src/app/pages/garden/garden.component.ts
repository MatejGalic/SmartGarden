import { Component } from '@angular/core';
import { GardenService } from 'src/app/core/services/garden.service';

@Component({
  selector: 'app-garden',
  templateUrl: './garden.component.html',
  styleUrl: './garden.component.scss',
})
export class GardenComponent {
  public gardenState$ = this.gardenSvc.gardenState$;

  constructor(private gardenSvc: GardenService) {}
}
