import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { Observable, take } from 'rxjs';
import { SolarPanelParameters } from '../models/dtos/solarPanelParameters';
import { SolarPanelResults } from '../models/dtos/solarPanelResults';

@Injectable({
  providedIn: 'root',
})
export class CalculatorService {
  private readonly apiUrl;

  constructor(private http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    this.apiUrl = `${baseUrl}api/calculator`;
  }

  public calculateSolarPanelEnergyProduction(
    parameters: SolarPanelParameters
  ): Observable<SolarPanelResults> {
    return this.http
      .post<SolarPanelResults>(this.apiUrl, parameters)
      .pipe(take(1));
  }
}
