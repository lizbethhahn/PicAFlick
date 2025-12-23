import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BottomNavComponent } from './layout/bottom-nav/bottom-nav';
import { RouterOutlet } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.html',
  standalone: true,
  styleUrls: ['./app.scss'],
  imports: [FormsModule, 
            CommonModule, 
            BottomNavComponent,
            RouterOutlet]
})
export class AppComponent {
  protected title = 'PicAFlick';
}
