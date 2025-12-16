import { inject,TestBed } from '@angular/core/testing';
import { HttpTestingController, provideHttpClientTesting } from '@angular/common/http/testing';
import { SearchService, TmdbMovie, TmdbTvShow } from './search.service';
import { provideHttpClient } from '@angular/common/http';

describe('SearchService', () => {
	beforeEach(() => {
		TestBed.configureTestingModule({
			providers: [
				SearchService,
				provideHttpClient(),
				provideHttpClientTesting()
			],
		});
  });

	it('should GET movies from the correct URL', inject(
    [SearchService, HttpTestingController],
    (service: SearchService, httpMock: HttpTestingController) => {
      const mockMovies: TmdbMovie[] = [{
        title: 'Star Wars',
        originial_title: 'Star Wars',
        overview: 'Princess Leia is captured and held hostage by the evil Imperial forces in their effort to take over the galactic Empire. Venturesome Luke Skywalker and dashing captain Han Solo team together with the loveable robot duo R2-D2 and C-3PO to rescue the beautiful princess and restore peace and justice in the Empire.',
        release_date: '1977-05-25',
        vote_average: 8.205,
        id: 0
      }];

      service.searchMovies('Star Wars').subscribe(movies => {
        expect(movies.length).toBe(1);
        expect(movies[0].originial_title).toBe('Star Wars');
      });

      const req = httpMock.expectOne('https://localhost:5000/api/Search/movie/Star Wars');
      expect(req.request.method).toBe('GET');
      req.flush(mockMovies);
    }
  ));

	it('should GET tv shows from the correct URL', inject(
    [SearchService, HttpTestingController],
    (service: SearchService, httpMock: HttpTestingController) => {
      const mockTvShows: TmdbTvShow[] = [{
        name: 'Firefly',
        original_name: 'Firefly',
        overview: 'In the year 2517, after the arrival of humans in a new star system, follow the adventures of the renegade crew of Serenity, a \"Firefly-class\" spaceship.',
        first_air_date: '2002-09-20',
        vote_average: 8.336,
        id: 0
      }];

      service.searchTvShows('Firefly').subscribe(tvshows => {
        expect(tvshows.length).toBe(1);
        expect(tvshows[0].original_name).toBe('Firefly');
      });

      const req = httpMock.expectOne('https://localhost:5000/api/Search/tv/Firefly');
      expect(req.request.method).toBe('GET');
      req.flush(mockTvShows);
    }
  ));
});
