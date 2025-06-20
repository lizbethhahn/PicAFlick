import { Component } from '@angular/core';
import { SearchService } from '../search.service';
import { TmdbMovie } from '../search.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

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
  isLoading: boolean = false;
  errorMessage: string = '';

  constructor(private searchService: SearchService) {}

  onSearch(): void {
    if (!this.searchTerm.trim()) {
      return; 
    }
    this.isLoading = true; 
    this.errorMessage = ''; 
  }

  searchMovies(query: string) {
    this.searchService.searchMovies(this.searchTerm).subscribe({
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
