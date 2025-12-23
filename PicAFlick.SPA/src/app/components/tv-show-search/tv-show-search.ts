import { Component } from '@angular/core';
import { SearchService } from '../../services/search.service';
import { TmdbTvShow } from '../../services/search.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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
  isLoading: boolean = false;
  errorMessage: string = '';

  constructor(private searchService: SearchService) {}

  onSearch(): void {
    if (!this.searchTerm.trim()) {
      return; 
    }
    this.isLoading = true; 
    this.errorMessage = ''; 

    this.searchTvShows(this.searchTerm);
  }
    
  searchTvShows(query: string) {
    this.searchService.searchTvShows(query).subscribe({
      next: (results) => {
        this.searchResults = results;  
        this.isLoading = false;  
      },
      error: (error) => {
        this.isLoading = false;  
        this.errorMessage = "An error occurred while searching"; 
        console.error(error);
      }
    });
  }
}
