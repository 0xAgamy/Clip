import { Component, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { Subscription } from 'rxjs';
import { AlertService } from '../../shared/alert/alert.service';
import { ModalService } from '../../services/modal.service';
import { IVideo } from '../model/ivideo';
import { VideoService } from '../services/video.service';

@Component({
  selector: 'app-manage',
  templateUrl: './manage.component.html',
  styleUrls: ['./manage.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false,
  // Per-component alert state so a message never leaks into another page.
  providers: [AlertService],
})
export class ManageComponent implements OnInit, OnDestroy {
  userId: string = '';
  private subscription!: Subscription;
  UserVideos: IVideo[] = [];

  // delete flow
  videoToDelete: IVideo | null = null;
  isDeleting = false;
  deleteError = '';

  constructor(
    private videoServices: VideoService,
    private _modalService: ModalService,
    private alert: AlertService,
  ) {}
  ngOnInit() {
    this._modalService.register('delete-video');
    this.loadVideos();
  }
  loadVideos() {
    this.subscription = this.videoServices.getUserClips().subscribe({
      next: (video) => {
        this.UserVideos = video;
      },
      error: () => {
        /* surfaced by the error interceptor */
      },
    });
  }
  RequestDelete(video: IVideo) {
    this.videoToDelete = video;
    this.deleteError = '';
    this._modalService.toggle('delete-video');
  }
  CancelDelete() {
    this.videoToDelete = null;
    this.deleteError = '';
    this.isDeleting = false;
    this._modalService.toggle('delete-video');
  }
  ConfirmDelete() {
    if (!this.videoToDelete || this.isDeleting) {
      return;
    }

    const id = this.videoToDelete.id;
    const title = this.videoToDelete.title;
    this.isDeleting = true;
    this.deleteError = '';

    this.videoServices.DeleteClip(id).subscribe({
      next: () => {
        this.UserVideos = this.UserVideos.filter((video) => video.id !== id);
        this.isDeleting = false;
        this.videoToDelete = null;
        this.alert.success(`"${title}" was deleted.`);
        this._modalService.toggle('delete-video');
      },
      error: () => {
        this.isDeleting = false;
        this.deleteError = 'Could not delete this video. Please try again.';
      },
    });
  }
  ngOnDestroy(): void {
    this.subscription.unsubscribe();
    this._modalService.unRegister('delete-video');
  }
}
