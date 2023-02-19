import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-succesful-send',
  templateUrl: './succesful-send.component.html',
  styleUrls: ['./succesful-send.component.scss']
})
export class SuccesfulSendComponent {

  @Input() text: string;

  _visible: boolean;

  @Input() set visible (value: boolean) {
    if (value) {
      this._visible = value;
      this.visibleChange.emit(this._visible);

      setTimeout(() => {
        this._visible = false;
        this.visibleChange.emit(this._visible);
      }, 1000);
    }
  }

  @Output() visibleChange: EventEmitter<boolean> = new EventEmitter<boolean>();
}
