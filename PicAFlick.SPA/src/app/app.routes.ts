import { Routes } from '@angular/router';

export const routes: Routes = [
  { path: '', redirectTo: 'search/movies', pathMatch: 'full' },

  // Lazy-loaded Movie Search component
  { path: 'search/movies',
    loadComponent: () =>
      import('./movie-search/movie-search').then(m => m.MovieSearchComponent)
  },

  // Lazy-loaded TV Show Search component
  { path: 'search/tv',
    loadComponent: () =>
      import('./tv-show-search/tv-show-search').then(m => m.TvShowSearchComponent)
  },

  { path: '**', redirectTo: 'search/movies' }
];
