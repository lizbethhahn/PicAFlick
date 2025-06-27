import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { TvShowSearchComponent } from './tv-show-search';
import { SearchService } from '../search.service';

describe('TvShowSearchComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TvShowSearchComponent],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting(),
        SearchService
      ]
    })
    .compileComponents();
  });

  it('should create component', () => {
    const fixture = TestBed.createComponent(TvShowSearchComponent)
    const component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });
});
