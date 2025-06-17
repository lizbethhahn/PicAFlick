import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MovieSearchComponent } from './movie-search';

describe('Search', () => {
  let component: MovieSearchComponent;
  let fixture: ComponentFixture<MovieSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MovieSearchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MovieSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
