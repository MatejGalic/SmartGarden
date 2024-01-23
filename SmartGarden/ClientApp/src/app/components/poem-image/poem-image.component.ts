import {
  ChangeDetectionStrategy,
  Component,
  ElementRef,
  EventEmitter,
  Input,
  Output,
  ViewChild,
} from '@angular/core';
import { Subject } from 'rxjs';
import { ContentPartDto } from 'src/app/core/models/autogenerated/dtos/contentPartDto';
import { ImagesService } from 'src/app/core/services/images.service';

@Component({
  selector: 'app-poem-image',
  templateUrl: './poem-image.component.html',
  styleUrl: './poem-image.component.scss',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PoemImageComponent {
  @Output() imageMouseEnter = new EventEmitter<MouseEvent>();
  @Output() imageMouseLeave = new EventEmitter<MouseEvent>();

  private _imageLoaded = new Subject<void>();
  public imageLoaded$ = this._imageLoaded.asObservable();

  protected mouseEventEnter = (event: MouseEvent) =>
    this.imageMouseEnter.emit(event);

  protected mouseEventLeave = (event: MouseEvent) =>
    this.imageMouseLeave.emit(event);

  protected imageLoadedEvent = (event: any) => this._imageLoaded.next();

  @ViewChild('poemImage') poemImage: ElementRef<HTMLImageElement>;
  public color: string;
  public red: string;
  public green: string;
  public blue: string;

  public opened: boolean = false;

  @Input() poemPart: ContentPartDto;
  @Input() canOpenDetails: boolean = false;
  public imagesApi: string = this.imagesSvc.apiUrl;

  constructor(private imagesSvc: ImagesService) {}

  public setDominantColor() {
    let color = this.imagesSvc.getAverageRGB(this.poemImage.nativeElement);
    this.color = `rgb(${color.r} ${color.g} ${color.b})`;
    this.red = color.r.toString();
    this.green = color.g.toString();
    this.blue = color.b.toString();
  }

  public close = () => (this.opened = false);
  public open = () => (this.opened = this.canOpenDetails);
}