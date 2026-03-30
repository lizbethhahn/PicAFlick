import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MediaChatService {
  sendMessage(message: string) {
  return this.http.post('https://localhost:7043/api/MediaChat', {
    message: message
  }, {
    responseType: 'text'
  });
}
  private http = inject(HttpClient);
}