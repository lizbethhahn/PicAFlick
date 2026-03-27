import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MediaChatService {
  sendMessage(message: string): Promise<string> {
    return Promise.resolve(`Got it. Tell me more`);
  }
}