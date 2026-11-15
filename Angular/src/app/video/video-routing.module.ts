import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EditComponent } from './edit/edit.component';
import { ManageComponent } from './manage/manage.component';
import { UploadComponent } from './upload/upload.component';
import {authGuard} from "../guards/auth-guard"

const routes: Routes = [
  {path:'manage',component:ManageComponent,
    data:{
      authOnly:true,
    },canActivate:[authGuard]
    
  },
  {path:'upload',component:UploadComponent,
    data:{
      authOnly:true,
    }
    ,canActivate:[authGuard]
  },


  {path:'edit/:id',component:EditComponent,
    data:{
      authOnly:true,
      
    }
    ,canActivate:[authGuard]
  }  

  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VideoRoutingModule { }
