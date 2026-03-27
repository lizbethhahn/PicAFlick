import { Component } from '@angular/core';
import { SearchService } from '../../services/search.service';
import { TmdbMovie } from '../../services/search.service';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';

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

  constructor(private searchService: SearchService, private router: Router) {}

  onSearch(): void {
    if (!this.searchTerm.trim()) {
      return; 
    }

    this.isLoading = true; 
    this.errorMessage = ''; 

    this.searchMovies(this.searchTerm);
  }

  searchMovies(query: string) {
    this.searchService.searchMovies(query).subscribe({
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

  onSelectMovie(movie: any) {
    this.router.navigate(['/media'], {
      state: { movie }
    });
  } 
}
