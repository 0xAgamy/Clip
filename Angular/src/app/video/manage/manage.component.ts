import { Component, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import {ModalService} from "../../services/modal.service"
import { IVideo } from '../model/ivideo';
import { VideoService } from '../services/video.service';

@Component({
    selector: 'app-manage',
    templateUrl: './manage.component.html',
    styleUrls: ['./manage.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class ManageComponent implements OnInit,OnDestroy {
userId:string=''
private subescripe!:Subscription
UserVideos:IVideo[]=[]
  constructor(private videoServices:VideoService, private _modalService:ModalService ) {

  }
ngOnInit() {
    this.subescripe= this.videoServices.getUserClips().subscribe({
      next:(video)=>{
        this.UserVideos=video;
        console.log(video)
      },
      error:(err)=>{
        // console.log(err)
      }
    })
}
DeleteVideo(id:number){
this.videoServices.DeleteClip(id).subscribe()
}
ngOnDestroy(): void {
    this.subescripe.unsubscribe();
}
}

