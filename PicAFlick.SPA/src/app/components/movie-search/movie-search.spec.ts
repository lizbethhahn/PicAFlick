import { TestBed } from '@angular/core/testing';
import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { MovieSearchComponent } from './movie-search';


describe('MovieSearchComponent', () => {
  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MovieSearchComponent],
      providers: [
        provideHttpClient(),
        provideHttpClientTesting()
      ]
    })
    .compileComponents();
  });

  it('should create component', () => {
    const fixture = TestBed.createComponent(MovieSearchComponent);
    const component = fixture.componentInstance;
    expect(component).toBeTruthy();
  });
});
