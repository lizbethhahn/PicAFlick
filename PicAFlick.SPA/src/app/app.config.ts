import { ApplicationConfig } from '@angular/core';
import { provideRouter, Routes } from '@angular/router';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

const routes: Routes = [
  {
    path: '',
    loadComponent: () =>
      import('./components/watchlist/watchlist')
        .then(m => m.WatchlistComponent),
  },
  {
    path: 'search/movies',
    loadComponent: () =>
      import('./components/movie-search/movie-search')
        .then(m => m.MovieSearchComponent),
  },
  {
    path: 'search/tv',
    loadComponent: () =>
      import('./components/tv-show-search/tv-show-search')
        .then(m => m.TvShowSearchComponent),
  },
  {
    path: 'media',
    loadComponent: () =>
      import('./components/media-chat/media-chat')
        .then(m => m.MediaChatComponent),
  },
  { path: '**', redirectTo: '' },
];

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),
  ],
};
