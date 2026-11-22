import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { IVideo } from '../video/model/ivideo';
import { VideoService } from '../video/services/video.service';

@Component({
  selector: 'app-clips-list',
  templateUrl: './clips-list.component.html',
  styleUrls: ['./clips-list.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false,
})
export class ClipsListComponent implements OnInit {
  Videos: IVideo[] = [];
  constructor(private videoServices: VideoService) {}

  ngOnInit(): void {
    this.videoServices.getVideos().subscribe((v) => {
      this.Videos = v;
    });
  }
}
