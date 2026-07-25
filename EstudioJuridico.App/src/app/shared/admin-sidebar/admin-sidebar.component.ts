import { Component, OnInit } from '@angular/core';
import { Router, RouterLink, NavigationEnd } from '@angular/router';
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/auth.service';
import { CasoService } from '../../services/caso.service';
import { filter } from 'rxjs/operators';

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
  menuAbierto = false;

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
    this.cargarConsultasPendientes();

    this.router.events.pipe(
      filter(e => e instanceof NavigationEnd)
    ).subscribe(() => {
      this.menuAbierto = false;
    });
  }

  cargarConsultasPendientes() {
    this.casoService.getConsultasPendientes().subscribe({
      next: (consultas) => {
        this.consultasPendientes = consultas.filter(
          (c: any) => !c.tieneRespuesta && !c.leida
        ).length;
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