import { Component, Input, ChangeDetectionStrategy } from '@angular/core';

@Component({
    selector: 'app-alert',
    templateUrl: './alert.component.html',
    styleUrls: ['./alert.component.css'],
    changeDetection: ChangeDetectionStrategy.Eager,
    standalone: false
})
export class AlertComponent {
COLOR_CLASSES={

  info:    "bg-indigo-900",
  success: "bg-green-600",
  error:   "bg-red-600",
} as const;
@Input() alertColor: keyof typeof this.COLOR_CLASSES = "info";

get bgcolor(): string {
  return this.COLOR_CLASSES[this.alertColor] ?? this.COLOR_CLASSES.info;
}

  
}
  