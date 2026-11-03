import { Component, Input, ChangeDetectionStrategy } from '@angular/core';

@Component({
    selector: 'app-alert',
    templateUrl: './alert.component.html',
    styleUrls: ['./alert.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class AlertComponent {
@Input()color:string='blue'

  constructor() {
    console.log(this.bgcolor)
  }
get bgcolor(){
    return `bg-${this.color}-400`
  }
  
}
  