import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TvShowSearchComponent } from './tv-show-search';

describe('TvShowSearchComponent', () => {
  let component: TvShowSearchComponent;
  let fixture: ComponentFixture<TvShowSearchComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [TvShowSearchComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(TvShowSearchComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
