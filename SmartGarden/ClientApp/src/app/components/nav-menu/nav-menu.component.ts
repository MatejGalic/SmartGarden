import { Location } from '@angular/common';
import { Component } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { EventType, NavigationEnd, Router } from '@angular/router';
import { filter, map } from 'rxjs';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.scss'],
})
export class NavMenuComponent {
  isHomepage$ = this.router.events.pipe(
    takeUntilDestroyed(),
    filter((e) => e.type === EventType.NavigationEnd),
    map((e) => {
      let endEvent = e as NavigationEnd;
      return endEvent.url === '/';
    })
  );

  constructor(private router: Router, private location: Location) {}

  goBack() {
    this.location.back();
  }
}
