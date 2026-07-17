import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { CasoService } from '../../services/caso.service';

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
  iniciales = 'AD';
  consultasPendientes = 0;

  constructor(public authService: AuthService, private router: Router, private casoService: CasoService) {
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
    this.casoService.getConsultasPendientes().subscribe({
      next: (consultas) => {
        this.consultasPendientes = consultas.filter((c: any) => !c.tieneRespuesta).length;
      },
      error: () => {}
    });
  }

  get rutaActiva(): string {
    return this.router.url;
  }

  logout() {
    this.authService.logout();
  }
}