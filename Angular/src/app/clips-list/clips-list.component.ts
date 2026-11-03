import { Component, OnDestroy, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { IVideo } from '../video/model/ivideo';
import { VideoService } from '../video/services/video.service';

@Component({
    selector: 'app-clips-list',
    templateUrl: './clips-list.component.html',
    styleUrls: ['./clips-list.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class ClipsListComponent implements OnInit,OnDestroy{
  Videos:IVideo[]=[];
  url:string=''
  constructor(private videoServices:VideoService) {
    this.url=videoServices.baseUrl
  }


  ngOnInit(): void {
      window.addEventListener('scroll',this.handleScrolling)

    
        this.videoServices.getVideos().subscribe(v=>{
          this.Videos=v
          console.log(this.Videos)
        });
    
  }

  handleScrolling=()=>{
    const {scrollTop,offsetHeight}= document.documentElement;
    const {innerHeight} = window
    const bottomOfWindo = Math.round(scrollTop) + innerHeight===offsetHeight
    if(bottomOfWindo)
      console.log('bootom')
    
  }
  ngOnDestroy(): void {
      
    window.removeEventListener('scroll',this.handleScrolling)
  }


}
