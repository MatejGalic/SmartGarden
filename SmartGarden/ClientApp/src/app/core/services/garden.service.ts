import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { BehaviorSubject, Observable, take, tap } from 'rxjs';
import { GardenParameters } from '../models/dtos/gardenParameters';
import { ToggleOpeningRequest } from '../models/dtos/toggleOpeningRequest';
import { SignalRService } from './signal-r.service';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';

@Injectable({
  providedIn: 'root',
})
export class GardenService {
  private readonly apiUrl;
  private _gardenState = new BehaviorSubject<GardenParameters>(null);
  public gardenState$ = this._gardenState.asObservable();

  constructor(
    private http: HttpClient,
    @Inject('BASE_URL') baseUrl: string,
    private signalRService: SignalRService
  ) {
    this.apiUrl = `${baseUrl}api/garden`;
    this.getLatestState().subscribe((state) => {
      this._gardenState.next(state);
    });

    this.signalRService.gardenState$
      .pipe(takeUntilDestroyed())
      .subscribe((state) => {
        this._gardenState.next(state);
      });
    this.signalRService.subscribeToGardenState();
  }

  public getLatestState(): Observable<GardenParameters> {
    return this.http.get<GardenParameters>(this.apiUrl).pipe(take(1));
  }

  public togglePump(state: ToggleOpeningRequest): Observable<GardenParameters> {
    return this.http
      .post<GardenParameters>(this.apiUrl + '/toggle-pump', state)
      .pipe(
        take(1),
        tap((state) => {
          this._gardenState.next(state);
        })
      );
  }

  public toggleWindows(
    state: ToggleOpeningRequest
  ): Observable<GardenParameters> {
    return this.http
      .post<GardenParameters>(this.apiUrl + '/toggle-windows', state)
      .pipe(
        take(1),
        tap((state) => {
          this._gardenState.next(state);
        })
      );
  }
}
