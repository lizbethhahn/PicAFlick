import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { inject } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MediaChatService {
  sendMessage(message: string, tmdbId?: number, mediaType?: string) {
  return this.http.post('https://localhost:7043/api/MediaChat', {
    message: message,
    tmdbId: tmdbId,
    mediaType: mediaType
  }, {
    responseType: 'text'
  });
}
  private http = inject(HttpClient);
}