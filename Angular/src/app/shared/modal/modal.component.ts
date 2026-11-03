import { Component, Input, ElementRef, OnInit, OnDestroy, ChangeDetectionStrategy } from '@angular/core';
import { ModalService } from '../../services/modal.service';

@Component({
    selector: 'app-modal',
    templateUrl: './modal.component.html',
    styleUrls: ['./modal.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class ModalComponent implements OnInit,OnDestroy {
@Input() MOdalId='';
  constructor(public modal:ModalService , public eleref:ElementRef) {
  }
    ngOnInit(){
      document.body.appendChild(this.eleref.nativeElement)
    }
    ngOnDestroy(): void {
        document.body.removeChild(this.eleref.nativeElement);
    }
    closeModal(){
      this.modal.toggleModel(this.MOdalId);
    }
}
