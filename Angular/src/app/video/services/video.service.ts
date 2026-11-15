import { HttpEvent, HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IVideo } from '../model/ivideo';
import { IVideoVm } from '../model/ivideo-vm';
import {API_BASE_URL} from '../../shared/api-shared'
@Injectable({
  providedIn: 'root'
})
export class VideoService {

  public baseUrl = API_BASE_URL

  constructor(private httpclient: HttpClient) {}


  getVideos():Observable<IVideo[]>{
    return this.httpclient.get<IVideo[]>(`${this.baseUrl}/api/Video`)
  }

  
  uploadVideo(payload: IVideoVm): Observable<HttpEvent<{ id: number }>> {
    const formData = new FormData();
    formData.append('videoFile', payload.VideoFile);
    formData.append('thumbnail', payload.thumbnail);
    formData.append('title', payload.title)
    return this.httpclient.post<{ id: number }>(`${this.baseUrl}/api/Video`, formData, {
      reportProgress: true,
      observe: 'events'
    });
  }



  getClip(id:string):Observable<IVideo>{
    return this.httpclient.get<IVideo>(`${this.baseUrl}/api/Video/${id}`)
  }

  getUserClips():Observable<IVideo[]>{
    return this.httpclient.get<IVideo[]>(`${this.baseUrl}/api/Video/user/`)
  }

  DeleteClip(id:number){
    return this.httpclient.delete(`${this.baseUrl}/api/Video/${id}`)
  }

  RecommencClips(id:number):Observable<IVideo[]>{
    return this.httpclient.get<IVideo[]>(`${this.baseUrl}/api/Video/recommend/${id}`)
  }
  EditClip(id:number,model:IVideoVm):Observable<IVideo>{
    return this.httpclient.put<IVideo>(`${this.baseUrl}/api/Video/${id}`,model)
  }
}
