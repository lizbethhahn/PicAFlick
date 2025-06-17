import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

export interface TmdbMovie {
  title: string;
  overview: string;
  release_date: string;
  vote_average: number;
}
export interface TmdbTvShow {
  title: string;
  overview: string;
  first_air_date: string;
  vote_average: number;
}

export interface TmdbMovieSearchResponse {
  page: number;
  results: { Results: TmdbMovie[] };  // results is an object with a Results array
  total_pages: number;
  total_results: number;
}

export interface TmdbTvShowSearchResponse {
  page: number;
  results: { Results: TmdbTvShow[] };  // results is an object with a Results array
  total_pages: number;
  total_results: number;
}

@Injectable({
  providedIn: 'root'
})

export class SearchService {
  private apiUrl = 'http://localhost:5000/api/search'
  constructor(private http: HttpClient) { }

  // Search for movies by title
  searchMovies(query: string): Observable<TmdbMovie[]> {
    return this.http.get<TmdbMovie[]>(`${this.apiUrl}/movie/${query}`)
  }

  // Search for tv shows by title
  searchTvShows(query: string): Observable<TmdbTvShow[]> {
    return this.http.get<TmdbTvShow[]>(`${this.apiUrl}/tvShow/${query}`)
  }
}
