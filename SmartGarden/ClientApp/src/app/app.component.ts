import { Component } from '@angular/core';
import { FaConfig, FaIconLibrary } from '@fortawesome/angular-fontawesome';
import { far } from '@fortawesome/free-regular-svg-icons';
import { fas } from '@fortawesome/free-solid-svg-icons';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent {
  constructor(library: FaIconLibrary, faConfig: FaConfig) {
    library.addIconPacks(fas);
    library.addIconPacks(far);
    faConfig.defaultPrefix = 'fas';
    faConfig.fixedWidth = true;
  }
}
