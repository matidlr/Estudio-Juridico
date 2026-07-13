import { Component, OnInit } from '@angular/core';
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
export class AdminSidebarComponent implements OnInit {
  nombreUsuario = '';
  emailUsuario = '';
  iniciales = '';
  esSuperAdmin = false;

  constructor(public authService: AuthService, private router: Router) {}

  ngOnInit() {
  const token = this.authService.getToken();
  if (token) {
    const payload = JSON.parse(atob(token.split('.')[1]));
    this.emailUsuario = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ?? '';
    this.nombreUsuario = payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] ?? this.emailUsuario.split('@')[0];
    this.esSuperAdmin = this.authService.getRol() === 'SuperAdmin';
    this.iniciales = this.nombreUsuario.substring(0, 2).toUpperCase();
  }
}

  get rutaActiva(): string {
    return this.router.url;
  }

  logout() {
    this.authService.logout();
  }
}