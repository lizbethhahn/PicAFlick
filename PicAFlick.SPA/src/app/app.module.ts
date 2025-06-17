// src/app/app.module.ts
import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser'; // Required for running Angular in the browser
import { FormsModule } from '@angular/forms';  // Needed if you're using ngModel for two-way data binding
import { App } from './app'; // Main root component
import { MovieSearchComponent } from './movie-search/movie-search'; // Movie search component
import { TvShowSearchComponent } from './tv-show-search/tv-show-search'; // TV show search component

@NgModule({
  declarations: [
    MovieSearchComponent, // Declare movie search component
    TvShowSearchComponent // Declare TV show search component
  ],
  imports: [
    App,
    BrowserModule,  // To run Angular in the browser
    FormsModule     // For two-way data binding (ngModel)
  ],
  providers: []
})
export class AppModule { }
