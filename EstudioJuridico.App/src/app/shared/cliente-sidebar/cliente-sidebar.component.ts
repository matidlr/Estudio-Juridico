import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { filter } from 'rxjs/operators';

@Component({
  selector: 'app-cliente-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './cliente-sidebar.component.html',
  styleUrl: './cliente-sidebar.component.scss'
})
export class ClienteSidebarComponent implements OnInit {
  nombreUsuario = '';
  emailUsuario = '';
  iniciales = 'CL';
  menuAbierto = false;

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

  ngOnInit() {
    this.router.events.pipe(
      filter(e => e instanceof NavigationEnd)
    ).subscribe(() => {
      this.menuAbierto = false;
    });
  }

  get rutaActiva(): string {
    return this.router.url;
  }

  logout() {
    this.authService.logout();
  }
}