import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TvShowSearch } from './tv-show-search';

describe('TvShowSearch', () => {
  let component: TvShowSearch;
  let fixture: ComponentFixture<TvShowSearch>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TvShowSearch]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TvShowSearch);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
