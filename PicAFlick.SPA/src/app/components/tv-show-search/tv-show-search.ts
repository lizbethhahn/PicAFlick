import { Component } from '@angular/core';
import { SearchService } from '../../services/search.service';
import { TmdbTvShow } from '../../services/search.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { WatchlistItem } from '../../models/watchlist-item';
import { WatchlistService } from '../../services/watchlist.service';
import { MediaType } from '../../models/media-type';

@Component({
  selector: 'app-tv-show-search',
  standalone: true,
  templateUrl: './tv-show-search.html',
  styleUrl: './tv-show-search.scss',
  imports: [CommonModule, FormsModule]
})

export class TvShowSearchComponent {
  searchTerm: string = '';
  searchResults: TmdbTvShow[] = [];
  selectedTvShows: TmdbTvShow[] = [];
  watchlistItems: WatchlistItem[] = [];
  addedTvShowCount: number = 0;
  isLoading: boolean = false;
  errorMessage: string = '';

  constructor(
    private searchService: SearchService, 
    private router: Router,
    private watchlistService: WatchlistService
  ) {}

  ngOnInit(): void {
    this.searchTerm = this.searchService.tvShowSearchTerm;
    this.searchResults = this.searchService.tvShowSearchResults;

    this.loadWatchlist();
  }

  searchTvShows(query: string) {
    this.searchService.searchTvShows(query).subscribe({
      next: (results) => {
        this.searchResults = results; 
        this.searchService.movieSearchTerm = this.searchTerm;
        this.searchService.tvShowSearchResults = results;
        this.isLoading = false;  
      },
      error: (error) => {
        this.isLoading = false;  
        this.errorMessage = "An error occurred while searching"; 
        console.error(error);
      }
    });
  }

  onTvShowSelectionChange(show: TmdbTvShow, event: Event) {
    const checkbox = event.target as HTMLInputElement;

    if (checkbox.checked) {
      
      if (this.selectedTvShows.length === 0) {
        this.addedTvShowCount = 0;
      }
    
    this.selectedTvShows.push(show);

    } else {
      this.selectedTvShows = this.selectedTvShows.filter(
        selectedTvShow => selectedTvShow.id !== show.id
      );
    }
  }

  addSelectedToWatchlist() {
    this.addedTvShowCount = 0;

    this.selectedTvShows.forEach(show => {
      const item = {
        tmdbId: show.id,
        title: show.name,
        mediaType: MediaType.TvShow,
        posterPath: show.poster_path,
        overview: show.overview,
        releaseDate: show.first_air_date || null,
        notes: null
      };

      this.watchlistService.add(item).subscribe({
        next: (createdItem) => {
          if (!this.watchlistItems.some(item => item.id === createdItem.id)) {
            this.watchlistItems.push(createdItem);
          }
          this.addedTvShowCount++;

          this.selectedTvShows = this.selectedTvShows.filter(
            tvShow => tvShow.id !== createdItem.tmdbId
          );
        },
        error: (error) => {
          console.error('Could not add to watchlist:', error);
        }
      });
    });
  }

  loadWatchlist(): void {
    this.watchlistService.getAll().subscribe({
      next: (items) => {
        this.watchlistItems = items;
      },
      error: (error) => {
        console.error('Could not load watchlist:', error);
      }
    });
  }  
  
  isInWatchlist(show: TmdbTvShow): boolean {
    return this.watchlistItems.some(
      item =>
        item.tmdbId === show.id &&
        item.mediaType === MediaType.TvShow
    );
  }

  onSearch(): void {
    if (!this.searchTerm.trim()) {
      return; 
    }
    this.isLoading = true; 
    this.errorMessage = ''; 

    this.searchTvShows(this.searchTerm);
  }

  onSelectShow(show: any) {
    this.router.navigate(['/media'], {
      state: { media: show }
    });
  }
}
