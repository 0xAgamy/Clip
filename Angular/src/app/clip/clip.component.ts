import { Component, OnInit, ViewChild, ElementRef, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute , Params} from '@angular/router';
import { IVideo } from '../video/model/ivideo';
import { VideoService } from '../video/services/video.service';
@Component({
    selector: 'app-clip',
    templateUrl: './clip.component.html',
    styleUrls: ['./clip.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class ClipComponent implements OnInit {
id=''
video!:IVideo;

topVideo:IVideo[]=[]
@ViewChild('videoPlayer',{static:true}) target?:ElementRef
constructor( public router:ActivatedRoute,private videoServices:VideoService) {
    }
    ngOnInit(): void {
    this.router.params.subscribe((params:Params)=>{
      this.id=params['id']
      this.videoServices.getClip(this.id).subscribe(v=>{
        this.video=v
        console.log(this.video)
      });
      let ID =+this.id
      this.videoServices.RecommencClips(ID).subscribe(v=>{
        this.topVideo=v
      })

    })
    


    }
}
