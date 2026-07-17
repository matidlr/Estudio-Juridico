import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { AdminSidebarComponent } from '../../../shared/admin-sidebar/admin-sidebar.component';
import { CasoService } from '../../../services/caso.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-notificaciones',
  standalone: true,
  imports: [CommonModule,  FormsModule, RouterLink, AdminSidebarComponent],
  templateUrl: './notificaciones.component.html',
  styleUrl: './notificaciones.component.scss'
})
export class NotificacionesComponent implements OnInit {
  proximos: any[] = [];
  ultimas: any[] = [];
  consultas: any[] = [];
  consultasSinRespuesta: any[] = [];
  cargando = true;
  error = '';
  tabActiva = 'vencimientos';

  consultaSeleccionada: any = null;
  respuesta = '';
  enviandoRespuesta = false;

  constructor(private casoService: CasoService) {}

  ngOnInit() {
    this.casoService.getProximos().subscribe({
      next: (proximos) => {
        this.proximos = proximos;
        this.cargando = false;
      },
      error: () => {
        this.error = 'Error al cargar las notificaciones.';
        this.cargando = false;
      }
    });

    this.casoService.getUltimasActualizaciones().subscribe({
      next: (ultimas) => this.ultimas = ultimas,
      error: () => {}
    });

    this.casoService.getConsultasPendientes().subscribe({
      next: (consultas) => {
        this.consultas = consultas;
        this.consultasSinRespuesta = consultas.filter((c: any) => !c.tieneRespuesta);
      },
      error: () => {}
    });
  }

  getDiasRestantes(fecha: string): number {
    const hoy = new Date();
    const fechaEvento = new Date(fecha);
    const diff = fechaEvento.getTime() - hoy.getTime();
    return Math.ceil(diff / (1000 * 60 * 60 * 24));
  }

  getColorDias(dias: number): string {
    if (dias <= 3) return 'urgente';
    if (dias <= 7) return 'proximo';
    return 'normal';
  }

  responderConsulta() {
  if (!this.respuesta.trim() || !this.consultaSeleccionada) return;
  this.enviandoRespuesta = true;

  this.casoService.responderComentario(this.consultaSeleccionada.casoId, this.respuesta).subscribe({
    next: () => {
      this.respuesta = '';
      this.enviandoRespuesta = false;
      this.consultaSeleccionada.tieneRespuesta = true;
      this.consultasSinRespuesta = this.consultasSinRespuesta.filter(
        c => c.id !== this.consultaSeleccionada.id
      );
      this.consultaSeleccionada = null;
    },
    error: () => {
      this.enviandoRespuesta = false;
    }
  });
}

ignorarConsulta(id: number, event: Event) {
  event.stopPropagation();
  if (!confirm('¿Ignorar esta consulta? Se eliminará de las notificaciones.')) return;

  this.casoService.eliminarComentario(id).subscribe({
    next: () => {
      this.consultas = this.consultas.filter(c => c.id !== id);
      this.consultasSinRespuesta = this.consultasSinRespuesta.filter(c => c.id !== id);
      if (this.consultaSeleccionada?.id === id) {
        this.consultaSeleccionada = null;
      }
    },
    error: () => {}
  });
}

marcarLeida(id: number, event: Event) {
  event.stopPropagation();

  this.casoService.marcarComentarioLeido(id).subscribe({
    next: () => {
      const consulta = this.consultas.find(c => c.id === id);
      if (consulta) consulta.leida = true;
      this.consultasSinRespuesta = this.consultasSinRespuesta.filter(c => c.id !== id);
      if (this.consultaSeleccionada?.id === id) {
        this.consultaSeleccionada = null;
      }
    },
    error: () => {}
  });
}
}