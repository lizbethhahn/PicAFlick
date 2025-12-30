import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive} from '@angular/router';

@Component({
  selector: 'app-nav-bar',
  standalone: true,
  templateUrl: './nav-bar.html',
  styleUrl: './nav-bar.scss',
  imports: [RouterLink, RouterLinkActive],
})
export class NavBarComponent {

}
