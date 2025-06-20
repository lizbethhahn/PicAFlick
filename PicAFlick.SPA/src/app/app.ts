import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MovieSearchComponent } from './movie-search/movie-search';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: true,
  styleUrls: ['./app.scss'],
  imports: [FormsModule, CommonModule, MovieSearchComponent]
})
export class App {
  protected title = 'PicAFlick';
}
