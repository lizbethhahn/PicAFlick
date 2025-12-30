import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { NavBarComponent } from './layout/nav-bar/nav-bar';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: true,
  styleUrls: ['./app.scss'],
  imports: [FormsModule,
    CommonModule,
    NavBarComponent,
    RouterOutlet, NavBarComponent]
})
export class AppComponent {
  protected title = 'PicAFlick';
}
