import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { NavbarComponent } from '../../../shared/navbar/navbar.component';
import { ClienteService } from '../../../services/cliente.service';
import { NotificacionService } from '../../../services/notificacion.service';

@Component({
  selector: 'app-mi-perfil',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, NavbarComponent],
  templateUrl: './mi-perfil.component.html',
  styleUrl: './mi-perfil.component.scss'
})
export class MiPerfilComponent implements OnInit {
  perfil: any = null;
  preferencias: any = { recibirPorEmail: true, recibirPorWhatsApp: false };
  cargando = true;
  guardando = false;
  error = '';
  exito = '';

  constructor(
    private clienteService: ClienteService,
    private notificacionService: NotificacionService
  ) {}

  ngOnInit() {
    this.clienteService.getMiPerfil().subscribe({
      next: (perfil) => {
        this.perfil = perfil;
        this.cargando = false;
      },
      error: () => {
        this.error = 'Error al cargar el perfil.';
        this.cargando = false;
      }
    });

    this.notificacionService.getPreferencias().subscribe({
      next: (prefs) => this.preferencias = prefs,
      error: () => {}
    });
  }

  guardarPerfil() {
    this.guardando = true;
    this.error = '';
    this.exito = '';

    this.clienteService.actualizarMiPerfil(this.perfil).subscribe({
      next: () => {
        this.exito = 'Perfil actualizado correctamente.';
        this.guardando = false;
        setTimeout(() => this.exito = '', 3000);
      },
      error: () => {
        this.error = 'Error al guardar el perfil.';
        this.guardando = false;
      }
    });
  }

  guardarPreferencias() {
    this.notificacionService.actualizarPreferencias(this.preferencias).subscribe({
      next: () => {
        this.exito = 'Preferencias guardadas correctamente.';
        setTimeout(() => this.exito = '', 3000);
      },
      error: () => {
        this.error = 'Error al guardar las preferencias.';
      }
    });
  }
}