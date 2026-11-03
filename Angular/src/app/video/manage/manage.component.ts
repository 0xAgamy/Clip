import { Component, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { ActivatedRoute, Params, Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { UserServicesService } from '../../user/UserServices/user-services.service';
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
url
  constructor(private router:Router , private activeRouter:ActivatedRoute ,private videoServices:VideoService,private UserServices:UserServicesService) {
    this.url=this.videoServices.baseUrl
  }
ngOnInit() {
    // this.activeRouter.queryParamMap.subscribe(  (params:Params)=>{
    //   this.videoOrder=params['sort']=='2'? params['sort']:'1'

    // }  )
    this.UserServices.user$.subscribe(u=>{
      this.userId=u!.id
      console.log(this.userId )
    })

   this.subescripe= this.videoServices.getUserClips(this.userId).subscribe(uv=>{
      console.log(this.userId )
      this.UserVideos=uv;
      console.log(this.UserVideos)
    })
}
DeleteVideo(id:number){
this.videoServices.DeleteClip(id).subscribe()
}
ngOnDestroy(): void {
    this.subescripe.unsubscribe();
}
  //You must do sorting in tha backend  
  // short(event:Event){
  //     let {value}  = (event.target as HTMLSelectElement);

  //     this.router.navigate([],{
  //       relativeTo:this.activeRouter,
  //       queryParams:{
  //         sort:value
  //       }
  //     })
  // }


}
