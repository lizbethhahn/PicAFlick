import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from './nav/nav';
import { MovieSearchComponent } from './movie-search/movie-search';
import { TvShowSearchComponent } from './tv-show-search/tv-show-search';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: true,
  styleUrls: ['./app.scss'],
  imports: [
    FormsModule, 
    CommonModule, 
    RouterOutlet, 
    NavComponent]
})
export class AppComponent {
  protected title = 'PicAFlick';
}
