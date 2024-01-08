import { Component } from '@angular/core';
import { SolarPanelParameters } from '../core/models/dtos/solarPanelParameters';
import { SolarPanelResults } from '../core/models/dtos/solarPanelResults';
import { CalculatorService } from '../core/services/calculator.service';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html',
})
export class FetchDataComponent {
  public results: SolarPanelResults;

  constructor(private calculatorSvc: CalculatorService) {
    const parameters: SolarPanelParameters = {
      // testing
      year: 2023,
      month: 5,
      day: 15,
      latitude: 45.5,
      panelTilt: 35.0,
      panelPower: 15,
      nOCT: 47.0,
      ambientTemperature: 15.0,
      panelEfficiency: 11.9,
      panelArea: 0.1575,
      powerTemperatureCoefficient: -0.5,
    };

    this.calculatorSvc
      .calculateSolarPanelEnergyProduction(parameters)
      .subscribe((res) => {
        this.results = res;
      });
  }
}
