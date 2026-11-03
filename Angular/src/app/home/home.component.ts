import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { IVideo } from '../video/model/ivideo';
import { VideoService } from '../video/services/video.service';

@Component({
    selector: 'app-home',
    templateUrl: './home.component.html',
    styleUrls: ['./home.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class HomeComponent {
   
  
  constructor() {
    
  }
 
}
