import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { WatchlistItem } from '../../models/watchlist-item';
import { WatchlistService } from '../../services/watchlist.service';

@Component({
  selector: 'app-watchlist',
  standalone: true,
  templateUrl: './watchlist.html',
  styleUrl: './watchlist.scss',
  imports: [CommonModule, FormsModule]
})

export class WatchlistComponent {
  watchlist: WatchlistItem[] = [];

  constructor(private watchlistService: WatchlistService) {}

  ngOnInit(): void {
    this.getAllWatchlistItems();
  }

  getAllWatchlistItems() {
    this.watchlistService.getAll().subscribe({
      next: (results) => {
        console.log('Watchlist results:', results);
        this.watchlist = results;
      },
      error: (error) => {
        console.error('Watchlist error:', error);
      }
    });
  }
}
