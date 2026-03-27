import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ElementRef, ViewChild } from '@angular/core';

@Component({
  selector: 'app-media-chat',
  imports: [CommonModule, FormsModule],
  templateUrl: './media-chat.html',
  styleUrl: './media-chat.scss',
})
  
export class MediaChatComponent {
  @ViewChild('chatContainer') private chatContainer!: ElementRef;

  movie = {
    title: 'Wonder Woman',
    release_date: '2017-05-30',
    poster_path: '/8UlWHLMpgZm9bx6QYh0NFoq67TZ.jpg'
  };

  messages = [
    {
      sender: 'user',
      text: 'I want something exciting but not too heavy.'
    },
    {
      sender: 'bot',
      text: 'Based on this pick, you might be in the mood for something adventurous with strong character energy.'
    }
  ];

  newMessage = '';
  sendMessage() {
    if (!this.newMessage.trim()) return;

    this.messages.push({
      sender: 'user',
      text: this.newMessage
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


