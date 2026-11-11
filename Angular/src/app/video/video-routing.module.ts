import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { UserServices } from '../user/UserServices/user-services.service';
import { EditComponent } from './edit/edit.component';
import { ManageComponent } from './manage/manage.component';
import { UploadComponent } from './upload/upload.component';

const routes: Routes = [
  {path:'manage',component:ManageComponent,
    data:{
      authOnly:true,
    },
  },
  {path:'upload',component:UploadComponent,
    data:{
      authOnly:true,
    }
  },


  {path:'Edit/:id',component:EditComponent,
    data:{
      authOnly:true,
    }
  }  

  
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class VideoRoutingModule { }
