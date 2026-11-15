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

// delete flow
videoToDelete:IVideo|null=null
isDeleting=false
deleteError=''
successMessage=''

  constructor(private videoServices:VideoService, private _modalService:ModalService ) {

  }
ngOnInit() {
    this._modalService.register('delete-video');
    this.loadVideos();
}
loadVideos(){
    this.subescripe= this.videoServices.getUserClips().subscribe({
      next:(video)=>{
        this.UserVideos=video;
      },
      error:(err)=>{
        // console.log(err)
      }
    })
}
RequestDelete(video:IVideo){
  this.videoToDelete=video;
  this.deleteError='';
  this._modalService.toggleModel('delete-video');
}
CancelDelete(){
  this.videoToDelete=null;
  this.deleteError='';
  this.isDeleting=false;
  this._modalService.toggleModel('delete-video');
}
ConfirmDelete(){
  if(!this.videoToDelete || this.isDeleting){return;}

  const id=this.videoToDelete.id;
  const title=this.videoToDelete.title;
  this.isDeleting=true;
  this.deleteError='';

  this.videoServices.DeleteClip(id).subscribe({
    next:()=>{
      this.UserVideos=this.UserVideos.filter(video=>video.id !== id);
      this.isDeleting=false;
      this.videoToDelete=null;
      this.successMessage=`"${title}" was deleted.`;
      this._modalService.toggleModel('delete-video');
    },
    error:()=>{
      this.isDeleting=false;
      this.deleteError='Could not delete this video. Please try again.';
    }
  })
}
ngOnDestroy(): void {
    this.subescripe.unsubscribe();
    this._modalService.unRegister('delete-video');
}
}

