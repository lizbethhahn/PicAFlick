// src/app/models/tmdb-search-response.ts
import { TmdbMovieDto } from './tmdb-movie';
import { TmdbTvShowDto } from './tmdb-tv-show';

export interface TmdbMovieSearchResponseDto {
  page: number;
  results: TmdbMovieDto[];
  total_pages: number;
  total_results: number;
}

export interface TmdbTvShowSearchResponseDto {
  page: number;
  results: TmdbTvShowDto[];
  total_pages: number;
  total_results: number;
}