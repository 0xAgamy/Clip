import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { IRegister } from '../userModels/register';
import { delay, map, Observable, of, ReplaySubject,filter ,switchMap} from 'rxjs';
import { ILogin } from '../userModels/login';
import { IUser } from '../userModels/iuser';
import { ActivatedRoute, Router , NavigationEnd, Data } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class UserServicesService {
  httpOptions
  private redirect=false;
  private userSource= new ReplaySubject<IUser|null>(1);
  public user$=this.userSource.asObservable();
  public userwithdelay$=this.user$.pipe(
    delay(1000)
  )
  userName:string='';
  constructor(private httpClient:HttpClient,private router:Router, public activeRoute:ActivatedRoute)  {
    this.httpOptions = {
      headers: new HttpHeaders({
        'Content-Type':  'application/json',
        //Authorization: 'my-auth-token'
      })
    };

    this.router.events.pipe(
      filter(e=>e instanceof NavigationEnd),
      map(e=>this.activeRoute.firstChild),
      switchMap(route=>route?.data ?? of({}))
    ).subscribe((data: Data)=>{
      this.redirect=data['authOnly'] ?? false
    });
  }
  
  register(modal:IRegister):Observable<IRegister>{
    return this.httpClient.post<IRegister>("https://localhost:7120/api/Account/Register",modal,this.httpOptions)
  }

  Login(modal:ILogin){
    return this.httpClient.post<IUser>("https://localhost:7120/api/Account/Login",JSON.stringify(modal),this.httpOptions)
      .pipe(
        map((user:IUser)=>{
          if(user){
            this.setUser(user);
            this.userName=user.userName
          }
        })
        
      )
  }
  Logout(){
    localStorage.removeItem('userkey');

    this.userSource.next(null);
    if(this.redirect){
      this.router.navigateByUrl("/");
    }
    
  }
  getJWT(){
    const key = localStorage.getItem('userkey');
    if(key){
      const userwithJwt :IUser = JSON.parse(key);
      return userwithJwt.jwt
    }
    else{
      return null
    }  
    
  }
  RefreshUser(jwt: string | null){
    if(jwt===null){
      this.userSource.next(null);
      return of (undefined)
    } 
    let headers = new HttpHeaders();
    headers=headers.set('Authorization','Bearer ' + jwt);
    return this.httpClient.get<IUser>("https://localhost:7120/api/Account/Refresh-User-Token",{headers})
    .pipe(
      map((user:IUser)=>{
        if(user){
          this.setUser(user)
        }
      })
    )
    ;
  }
  private setUser(user:IUser){
    localStorage.setItem('userkey',JSON.stringify(user))
    this.userSource.next(user);
    
    
  }
}
