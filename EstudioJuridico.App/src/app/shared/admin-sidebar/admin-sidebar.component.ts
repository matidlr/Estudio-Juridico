import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-admin-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './admin-sidebar.component.html',
  styleUrl: './admin-sidebar.component.scss'
})
export class AdminSidebarComponent {
  nombreUsuario = '';
  emailUsuario = '';
  iniciales = 'AD';

  constructor(public authService: AuthService, private router: Router) {
    const token = authService.getToken();
    if (token) {
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        this.emailUsuario = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ?? '';
        this.nombreUsuario = this.emailUsuario.split('@')[0];
        this.iniciales = this.nombreUsuario.substring(0, 2).toUpperCase();
      } catch {}
    }
  }

  get rutaActiva(): string {
    return this.router.url;
  }

  logout() {
    this.authService.logout();
  }
}