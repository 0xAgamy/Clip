import { Component, ElementRef, OnDestroy, ViewChild } from '@angular/core';
import { HttpEvent, HttpEventType } from '@angular/common/http';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AlertService } from '../../shared/alert/alert.service';
import { VideoService } from '../services/video.service';
import { IVideoVm } from '../model/ivideo-vm';

type UploadState = 'idle' | 'selected' | 'uploading' | 'done' | 'error';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css'],
  standalone: false,
})
export class UploadComponent implements OnDestroy {
  // upload flow state
  uploadState: UploadState = 'idle';
  progress = 0;

  // step 1 (file selection)
  isDragOver = false;
  nextStep = false;

  // files
  videoFile: File | null = null;
  thumbnailFile: File | null = null;

  // object-URL previews (constant memory — never base64)
  videoPreviewUrl: string | null = null;
  thumbnailPreviewUrl: string | null = null;
  private videoObjectUrl: string | null = null;
  private thumbnailObjectUrl: string | null = null;

  title = new FormControl('', [Validators.required, Validators.minLength(3)]);
  videoForm = new FormGroup({ title: this.title });

  @ViewChild('videoPlayer') videoPlayer?: ElementRef<HTMLVideoElement>;

  constructor(
    private videoService: VideoService,
    private router: Router,
    private alert: AlertService,
  ) {}

  // ---- Step 1: select / drop the video -------------------------------------

  storeFile(event: Event): void {
    this.isDragOver = false;

    let file: File | null;
    if (event instanceof DragEvent) {
      file = event.dataTransfer?.files.item(0) ?? null;
    } else {
      const input = event.target as HTMLInputElement;
      file = input.files?.item(0) ?? null;
      input.value = ''; // allow re-selecting the same file later
    }
    if (!file) return;

    if (file.type !== 'video/mp4') {
      this.alert.error('Only MP4 videos are supported.');
      return;
    }

    this.resetPreview();
    this.videoFile = file;
    this.videoObjectUrl = URL.createObjectURL(file);
    this.videoPreviewUrl = this.videoObjectUrl;

    this.title.setValue(file.name.replace(/\.[^/.]+$/, ''));
    this.nextStep = true;
    this.uploadState = 'selected';
  }

  backToUpload(): void {
    this.nextStep = false;
    this.resetPreview();
    this.uploadState = 'idle';
    this.progress = 0;
  }

  // ---- Step 2: thumbnail from a video frame (canvas, no extra deps) --------

  onVideoLoaded(): void {
    if (!this.thumbnailFile) {
      this.generateThumbnail();
    }
  }

  async generateThumbnail(): Promise<void> {
    const video = this.videoPlayer?.nativeElement;
    if (!video || !video.videoWidth) return;

    try {
      const file = await this.captureThumbnail(video);
      if (!file) return;

      if (this.thumbnailObjectUrl) URL.revokeObjectURL(this.thumbnailObjectUrl);
      this.thumbnailFile = file;
      this.thumbnailObjectUrl = URL.createObjectURL(file);
      this.thumbnailPreviewUrl = this.thumbnailObjectUrl;
    } catch (err) {
      console.error('Thumbnail capture failed', err);
      this.alert.error('Could not capture a thumbnail from this video.');
    }
  }

  private async captureThumbnail(video: HTMLVideoElement): Promise<File | null> {
    if (!video.duration) return null;

    // 30% into the video is usually a good frame
    await this.seekTo(video, video.duration * 0.3);

    const maxWidth = 640;
    const scale = Math.min(1, maxWidth / video.videoWidth);
    const canvas = document.createElement('canvas');
    canvas.width = Math.round(video.videoWidth * scale);
    canvas.height = Math.round(video.videoHeight * scale);

    const context = canvas.getContext('2d');
    if (!context) return null;
    context.drawImage(video, 0, 0, canvas.width, canvas.height);

    const blob = await new Promise<Blob | null>((resolve) =>
      canvas.toBlob(resolve, 'image/jpeg', 0.8),
    );
    if (!blob) return null;

    return new File([blob], 'thumbnail.jpg', { type: 'image/jpeg' });
  }

  private seekTo(video: HTMLVideoElement, seconds: number): Promise<void> {
    return new Promise((resolve) => {
      const onSeeked = () => {
        video.removeEventListener('seeked', onSeeked);
        resolve();
      };
      video.addEventListener('seeked', onSeeked);
      video.currentTime = seconds;
    });
  }

  // ---- Step 3: upload with progress ----------------------------------------

  uploadVideo(): void {
    if (!this.videoFile || !this.thumbnailFile || this.videoForm.invalid) return;

    const payload = {
      VideoFile: this.videoFile,
      thumbnail: this.thumbnailFile,
      title: this.title.value ?? '',
    } as IVideoVm;

    this.alert.info('Please wait — your clip is being uploaded.');
    this.uploadState = 'uploading';
    this.progress = 0;

    this.videoService.uploadVideo(payload).subscribe({
      next: (event: HttpEvent<{ id: number }>) => {
        if (event.type === HttpEventType.UploadProgress && event.total) {
          this.progress = Math.round((100 * event.loaded) / event.total);
        } else if (event.type === HttpEventType.Response) {
          this.uploadState = 'done';
          this.alert.success('Upload complete! Redirecting...');
          if (event.body?.id != null) {
            this.router.navigate(['/clip', event.body.id]);
          }
        }
      },
      error: (err) => {
        this.uploadState = 'error';
        this.alert.error(
          typeof err?.error === 'string' ? err.error : 'Upload failed. Please try again.',
        );
        this.progress = 0;
      },
    });
  }

  // ---- cleanup ---------------------------------------------------------------

  private resetPreview(): void {
    if (this.videoObjectUrl) URL.revokeObjectURL(this.videoObjectUrl);
    if (this.thumbnailObjectUrl) URL.revokeObjectURL(this.thumbnailObjectUrl);
    this.videoObjectUrl = null;
    this.thumbnailObjectUrl = null;
    this.videoPreviewUrl = null;
    this.thumbnailPreviewUrl = null;
    this.videoFile = null;
    this.thumbnailFile = null;
  }

  ngOnDestroy(): void {
    this.resetPreview();
  }
}
