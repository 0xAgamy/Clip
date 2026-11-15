import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { IVideo } from '../model/ivideo';
import { VideoService } from '../services/video.service';
import { IVideoVm } from '../model/ivideo-vm';


@Component({
    selector: 'app-edit',
    templateUrl: './edit.component.html',
    styleUrls: ['./edit.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class EditComponent implements OnInit {
Video!:IVideo
id:string=''
//

showAlert=false;
alertColor='info';
alertMesg='Please Wait ! Your Clip is being uploaded.'
inSubmission=false;

  title = new FormControl('',[Validators.required,Validators.minLength(3)])
  videoForm=new FormGroup({
    title:this.title
  })
    constructor(private VideoServices:VideoService,
      private activeRouter:ActivatedRoute,
      ) {

    }

    ngOnInit() {
        this.activeRouter.paramMap.subscribe((paramsmap)=>{
            this.id=String(paramsmap.get('id'))
            this.VideoServices.getClip(this.id).subscribe(v=>{
            this.Video=v
            })
          })
    
            
        }

  
    get TitleName(){
    return this.Video.title
    }



  Edit(){
      const editedVideo={
        title:this.videoForm.get('title')?.value
      }as IVideoVm
      let ID = + this.id 
      console.log("edit")
      this.VideoServices.EditClip(ID,editedVideo).subscribe();
  }
}