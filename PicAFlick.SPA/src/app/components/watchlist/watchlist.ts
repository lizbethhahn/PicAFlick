import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { WatchlistItem } from '../../models/watchlist-item';
import { WatchlistService } from '../../services/watchlist.service';
import { MediaType } from '../../models/media-type';

@Component({
  selector: 'app-watchlist',
  standalone: true,
  templateUrl: './watchlist.html',
  styleUrl: './watchlist.scss',
  imports: [CommonModule, FormsModule]
})

export class WatchlistComponent {
  readonly MediaType = MediaType;
  watchlist: WatchlistItem[] = [];

  constructor(private watchlistService: WatchlistService) {}

  ngOnInit(): void {
    this.getAllWatchlistItems();
  }

  getAllWatchlistItems() {
    this.watchlistService.getAll().subscribe({
      next: (results) => {
        this.watchlist = results;
      },
      error: (error) => {
        console.error('Watchlist error:', error);
      }
    });
  }

  loadWatchlist(): void {
    this.watchlistService.getAll().subscribe({
      next: (items) => {
        this.watchlist = items;
      },
      error: (error) => {
        console.error('Could not load watchlist:', error);
      }
    });
  }

  removeFromWatchlist(itemId: number): void {
    const confirmed = confirm('Are you sure you want to remove this item from your watchlist?');
    if (!confirmed) {
      return;
    }
    this.watchlistService.remove(itemId).subscribe({
      next: () => {
        this.watchlist = this.watchlist.filter(item => item.id !== itemId) ;
      },
      
      error: (error) => {
        console.error('Could not remove item from watchlist:', error);
      }
    });
  }
}
