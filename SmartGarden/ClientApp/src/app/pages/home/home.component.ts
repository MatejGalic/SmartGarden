import { Component, inject } from '@angular/core';
import { ImagesService } from 'src/app/core/services/images.service';
import { PoemService } from 'src/app/core/services/poem.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrl: './home.component.scss',
})
export class HomeComponent {
  poems$ = this.poemService.getPoems();
  readonly apiUrl: string = inject(ImagesService).apiUrl;

  constructor(private poemService: PoemService) {}
}
