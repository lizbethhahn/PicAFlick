import { Component } from '@angular/core';
import { SearchService } from '../search.service';
import { TmdbTvShow } from '../search.service';

@Component({
  selector: 'app-tv-show-search',
  standalone: false,
  templateUrl: './tv-show-search.html',
  styleUrl: './tv-show-search.scss'
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

    // Call the search service to fetch results
    this.searchService.searchTvShows(this.searchTerm).subscribe({
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
