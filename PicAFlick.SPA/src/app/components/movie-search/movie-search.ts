import { Component } from '@angular/core';
import { SearchService } from '../../services/search.service';
import { TmdbMovie } from '../../services/search.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { WatchlistItem } from '../../models/watchlist-item';
import { WatchlistService } from '../../services/watchlist.service';
import { MediaType } from '../../models/media-type';

@Component({
  selector: 'app-movie-search',
  standalone: true,
  templateUrl: './movie-search.html',
  styleUrl: './movie-search.scss',
  imports: [CommonModule, FormsModule]
})

export class MovieSearchComponent {
  searchTerm: string = '';
  searchResults: TmdbMovie[] = [];
  selectedMovies: TmdbMovie[] = [];
  watchlistItems: WatchlistItem[] = [];
  addedMovieCount: number = 0;
  isLoading: boolean = false;
  errorMessage: string = '';

  constructor(
    private searchService: SearchService, 
    private router: Router, 
    private watchlistService: WatchlistService
  ) {}

  ngOnInit(): void {
    this.searchTerm = this.searchService.movieSearchTerm;
    this.searchResults = this.searchService.movieSearchResults;

    this.loadWatchlist();
  }
 
  searchMovies(query: string) {
    this.searchService.searchMovies(query).subscribe({
      next: (results) => {
        this.searchResults = results;  
        this.searchService.movieSearchTerm = this.searchTerm;
        this.searchService.movieSearchResults = results;
        this.isLoading = false;  
      },
      error: (error) => {
        this.isLoading = false;
        this.errorMessage = `Search failed: ${error.status} ${error.statusText}`;
        console.error('Movie search error:', error);
      }
    });  
  }

  onMovieSelectionChange(movie: TmdbMovie, event: Event) {
    const checkbox = event.target as HTMLInputElement;
    
    if (checkbox.checked) {
      
      if (this.selectedMovies.length === 0) {
        this.addedMovieCount = 0;
      }

      this.selectedMovies.push(movie);

    } else {
      this.selectedMovies = this.selectedMovies.filter(
        selectedMovie => selectedMovie.id !== movie.id
      );
    }
  }

  addSelectedToWatchlist() {
    this.addedMovieCount = 0;

    this.selectedMovies.forEach(movie => {
      const item = {
        tmdbId: movie.id,
        Title: movie.title,
        MediaType: MediaType.Movie,
        posterPath: movie.poster_path,
        overview: movie.overview,
        releaseDate: movie.release_date || null,
        notes: null
      };

      this.watchlistService.add(item).subscribe({
        next: (createdItem) => {
          if (!this.watchlistItems.some(item => item.id === createdItem.id)) {
            this.watchlistItems.push(createdItem);
          }                   
          this.addedMovieCount++;

          this.selectedMovies = this.selectedMovies.filter(
            movie => movie.id !== createdItem.tmdbId
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

  isInWatchlist(movie: TmdbMovie): boolean {
    return this.watchlistItems.some(
      item =>
        item.tmdbId === movie.id &&
        item.mediaType === MediaType.Movie
    );    
  }

  onSearch(): void {
    if (!this.searchTerm.trim()) {
      return; 
    }

    this.isLoading = true; 
    this.errorMessage = ''; 

    this.searchMovies(this.searchTerm);
  }

  onSelectMovie(movie: any) {
    this.router.navigate(['/media'], {
      state: { media: movie }
    });
  } 
}
