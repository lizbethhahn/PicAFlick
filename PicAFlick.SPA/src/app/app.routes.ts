import { Routes } from '@angular/router';

export const routes: Routes = [
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

