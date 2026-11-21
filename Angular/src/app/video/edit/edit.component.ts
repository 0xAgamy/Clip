import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AlertService } from '../../shared/alert/alert.service';
import { IVideo } from '../model/ivideo';
import { VideoService } from '../services/video.service';
import { IVideoVm } from '../model/ivideo-vm';

@Component({
  selector: 'app-edit',
  templateUrl: './edit.component.html',
  styleUrls: ['./edit.component.css'],
  changeDetection: ChangeDetectionStrategy.Eager,
  standalone: false,
})
export class EditComponent implements OnInit {
  Video!: IVideo;
  id: string = '';
  inSubmission = false;

  title = new FormControl('', [Validators.required, Validators.minLength(3)]);
  videoForm = new FormGroup({
    title: this.title,
  });
  constructor(
    private VideoServices: VideoService,
    private activeRouter: ActivatedRoute,
    private alert: AlertService,
  ) {}

  ngOnInit() {
    this.activeRouter.paramMap.subscribe((paramsmap) => {
      this.id = String(paramsmap.get('id'));
      this.VideoServices.getClip(this.id).subscribe((v) => {
        this.Video = v;
        this.videoForm.patchValue({ title: v.title });
      });
    });
  }

  get TitleName() {
    return this.Video?.title ?? '';
  }

  Edit() {
    if (this.videoForm.invalid || this.inSubmission) {
      return;
    }

    const editedVideo = {
      title: this.videoForm.get('title')?.value,
    } as IVideoVm;
    const ID = +this.id;

    this.inSubmission = true;
    this.alert.info('Saving your changes...');

    this.VideoServices.EditClip(ID, editedVideo).subscribe({
      next: (edited) => {
        this.inSubmission = false;
        if (edited) {
          this.Video = edited;
          this.videoForm.patchValue({ title: edited.title });
          this.videoForm.markAsPristine();
        }
        this.alert.success('Your clip was updated successfully.');
      },
      error: () => {
        this.inSubmission = false;
        this.alert.error('Could not update this clip. Please try again.');
      },
    });
  }
}
