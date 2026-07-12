import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class NotificationService {
  show(message: string) {
    // For now, we use a simple alert or just console.log.
    // In a real app we would use a toast or snackbar library.
    alert(message);
  }
}
