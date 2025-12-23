import { Component } from '@angular/core';
import { RouterLink, RouterLinkActive} from '@angular/router';

@Component({
  selector: 'app-bottom-nav',
  standalone: true,
  templateUrl: './bottom-nav.html',
  styleUrl: './bottom-nav.scss',
  imports: [RouterLink, RouterLinkActive],
})
export class BottomNavComponent {

}
