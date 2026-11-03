import { ThisReceiver } from '@angular/compiler';
import { Component, ElementRef, ViewChild, ChangeDetectionStrategy } from '@angular/core';
import { FormGroup,FormControl,Validators } from '@angular/forms';
import { delay, Observable, takeLast } from 'rxjs';
import { NgxCaptureService } from 'ngx-capture';
import { VideoService } from '../services/video.service';
import { IVideoVm } from '../model/ivideo-vm';
import { Route, Router } from '@angular/router';
import { UserServicesService } from '../../user/UserServices/user-services.service';
import { IUser } from '../../user/userModels/iuser';

@Component({
    selector: 'app-upload',
    templateUrl: './upload.component.html',
    styleUrls: ['./upload.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class UploadComponent {
  testthubmnailfile:File|null=null
  //
  UserEmail?:string=''
  @ViewChild('capture')capture?:ElementRef
  screenshot:string='';
  imgfile:File|null=null;
    showAlert=false;
    alertColor='blue';
    alertMesg='Please Wait ! Your Clip is being uploaded.'
    inSubmission=false;
      //  
      videosource?:string='';
      //
      nextstep=false;
      isDragOver=false;
      videofile:File|null=null

      title = new FormControl('',[Validators.required,Validators.minLength(3)])
      videoForm=new FormGroup({
        title:this.title
      })
     constructor(private ngxCap:NgxCaptureService
      ,private VideoServices:VideoService
      ,private Route:Router
      ,private userServices:UserServicesService) {
      this.userServices.user$.subscribe(user=>{
        this.UserEmail=user?.email
      })
      
     }
//

captureImages(){
      
  this.ngxCap.getImage(this.capture?.nativeElement,true).subscribe((img)=>{
    this.screenshot=img
      console.log(this.screenshot)
  })
}
dataURItoBlob(dataURI:string) {
  const byteString = window.atob(dataURI);
  const arrayBuffer = new ArrayBuffer(byteString.length);
  const int8Array = new Uint8Array(arrayBuffer);
  for (let i = 0; i < byteString.length; i++) {
    int8Array[i] = byteString.charCodeAt(i);
  }
  const blob = new Blob([int8Array], { type: 'image/png' });
  return blob;
}
converttofile(){
    const imgName= this.TitleName!.concat("Thubmnail.png");
    const im=this.screenshot.split(',')[1];
    const imgblob=this.dataURItoBlob(im);
    this.imgfile=new File([imgblob],imgName,{ type :'image/png' })
    
}





     //
      storeFile($event:Event){
        this.isDragOver=false;
        this.videofile=($event as DragEvent).dataTransfer?.files.item(0)??null
        if(!this.videofile || this.videofile.type!=='video/mp4')return
        let reader=new FileReader()
        reader.readAsDataURL(this.videofile)
        reader.onload=()=>{
          var bb64=reader.result?.toString()
          this.videosource=bb64;  
        }
        this.title.setValue(
          this.videofile.name.replace(/\.[^/.]+$/,''))   
              
        this.nextstep=true;
    }

    backToUpload(){
      this.nextstep=false;
    }

    uploadVideo(){
      this.converttofile()
      this.showAlert=true;
      this.alertColor='blue'
      this.alertMesg='Please Wait ! Your Clip is being uploaded.'
      this.inSubmission=true;

      let VideoVM={
      VideoFile:this.videofile,
      thumbnail:this.imgfile,
      userEmail:this.UserEmail,
      title:this.TitleName,
      }as IVideoVm

      console.log(VideoVM);
    this.VideoServices.uploadVideo(VideoVM).subscribe({
      
      next:(rse)=>{
        console.log(rse)
        console.log("resoponose")
      },
      error:(err)=>{
        console.log("there are an error")
        console.log(err)
      }
    });
      
      // if(this.inSubmission){
      
      //   this.Route.navigateByUrl("/") 
      // }

    }


    get TitleName(){
      return this.videoForm.get('title')?.value;
    }

    // Getthumbnail(event:Event){
    //     this.testthubmnailfile=  (event as DragEvent).dataTransfer?.files.item(0)??null
    //     if(this.testthubmnailfile!=null)
    //       console.log("threre is a file")
    //     console.log(this.testthubmnailfile)
    //     console.log("i'm in get Thumb")
    // }
  }





