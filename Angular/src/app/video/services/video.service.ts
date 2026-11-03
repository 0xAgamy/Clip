import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IVideo } from '../model/ivideo';
import { IVideoVm } from '../model/ivideo-vm';

@Injectable({
  providedIn: 'root'
})
export class VideoService {
  httpOptions
  public baseUrl = "https://localhost:7120/"
  
  constructor(private httpclient:HttpClient) { 
    this.httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        
        //Authorization: 'my-auth-token'
      })
    };
  }


  getVideos():Observable<IVideo[]>{
    return this.httpclient.get<IVideo[]>("https://localhost:7120/api/Video")
  }

  
  uploadVideo(modal:IVideoVm):Observable<IVideoVm>{
    const fromdata= new FormData();
    fromdata.append('videoFile',modal.VideoFile)
    fromdata.append('thumbnail',modal.thumbnail) 
    fromdata.append('userEmail',modal.userEmail)
    fromdata.append('title',modal.title)
    return this.httpclient.post<IVideoVm>("https://localhost:7120/api/Video",fromdata,{reportProgress:true})
  }



  getClip(id:string):Observable<IVideo>{
    return this.httpclient.get<IVideo>(`https://localhost:7120/api/Video/${id}`)
  }

  getUserClips(userId:string):Observable<IVideo[]>{
    const params =new HttpParams().set('userId',userId)
    return this.httpclient.get<IVideo[]>(`https://localhost:7120/api/Video/user/${userId}`)
  }

  DeleteClip(id:number){
    return this.httpclient.delete(`https://localhost:7120/api/Video/${id}`)
  }

  RecommencClips(id:number):Observable<IVideo[]>{
    return this.httpclient.get<IVideo[]>(`https://localhost:7120/api/Video/recommend/${id}`)
  }
  EditClip(id:number,model:IVideoVm){
    return this.httpclient.put(`https://localhost:7120/api/Video/${id}`,model)
  }
}
