import { HttpClient } from '@angular/common/http';
import { Inject, Injectable } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { BehaviorSubject, Observable, take } from 'rxjs';
import { GardenParameters } from '../models/dtos/gardenParameters';
import { SignalRService } from './signal-r.service';

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

  public openPump(): Observable<void> {
    return this.http.get<void>(this.apiUrl + '/open-pump').pipe(take(1));
  }

  public openWindows(): Observable<void> {
    return this.http.get<void>(this.apiUrl + '/open-windows').pipe(take(1));
  }
}
