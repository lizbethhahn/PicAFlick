import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ElementRef, ViewChild } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-media-chat',
  imports: [CommonModule, FormsModule],
  templateUrl: './media-chat.html',
  styleUrl: './media-chat.scss',
})
  
export class MediaChatComponent {
  @ViewChild('chatContainer') private chatContainer!: ElementRef;

  media: any;

  constructor(private router: Router) {
    const nav = this.router.currentNavigation();
    this.media =
        nav?.extras?.state?.['media'] ||
        history.state?.media;
  }

  messages = [
    {
      sender: 'bot',
      text: 'How can I help you?'
    }
  ];

  get mediaTitle() {
    return this.media?.title || this.media?.name;
  }

  get mediaYear() {
    const date = this.media?.release_date || this.media?.first_air_date;
    return date ? new Date(date).getFullYear() : '';
  }

  newMessage = '';
  sendMessage() {
  const userMessage = this.newMessage;
  const contextTitle = this.media?.title || this.media?.name;
  const messageToSend = `Talking about: ${contextTitle}\n\n${userMessage}`;

  if (!userMessage.trim()) return;
  this.messages.push({
    sender: 'user',
    text: userMessage
  });
  
  this.newMessage = '';

  setTimeout(() => {
    this.chatContainer.nativeElement.scrollTop =
      this.chatContainer.nativeElement.scrollHeight;
  });
}

  onKeyDown(event: KeyboardEvent) {
    if (event.key === 'Enter' && !event.shiftKey) {
      event.preventDefault();
      this.sendMessage();
    }
  }
}


