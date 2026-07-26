import { Component, OnInit } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.scss'
})
export class AppComponent implements OnInit {
  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
  setInterval(() => {
    const url = this.router.url;
    if (url.includes('login') || url.includes('admin-estudio') || url === '/') return;

    if (!this.authService.estaLogueado()) {
      const rol = localStorage.getItem('rol');
      if (rol === 'Admin' || rol === 'SuperAdmin' || rol === 'Abogado') {
        this.router.navigate(['/admin-estudio']);
      } else {
        this.router.navigate(['/login']);
      }
    }
  }, 60000);
}
}