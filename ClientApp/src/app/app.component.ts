import { Component } from '@angular/core';
import { NotificationService } from './notification.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html'
})
export class AppComponent {

  constructor(private notificationService: NotificationService) { }

  onConfirm(): void {
    // this.messageService.clear('errorDlg');
    this.notificationService.removeMessages();
  }

}
