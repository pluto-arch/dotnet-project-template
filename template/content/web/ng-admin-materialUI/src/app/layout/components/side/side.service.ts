import { Injectable, Output, EventEmitter } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SideService {

  isOpen = true;

  @Output() change: EventEmitter<boolean> = new EventEmitter<boolean>();

  toggle() {
    this.isOpen = !this.isOpen;
    this.change.emit(this.isOpen);
  }
}
