import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AdminSidebarComponent } from '../../../shared/admin-sidebar/admin-sidebar.component';
import { CasoService } from '../../../services/caso.service';

@Component({
  selector: 'app-calendario',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, AdminSidebarComponent],
  templateUrl: './calendario.component.html',
  styleUrl: './calendario.component.scss'
})
export class CalendarioComponent implements OnInit {
  recordatorios: any[] = [];
  cargando = true;
  error = '';

  // Vista del calendario
  hoy = new Date();
  mesActual = new Date().getMonth();
  anioActual = new Date().getFullYear();

  diasSemana = ['Dom', 'Lun', 'Mar', 'Mié', 'Jue', 'Vie', 'Sáb'];
  meses = [
    'Enero', 'Febrero', 'Marzo', 'Abril', 'Mayo', 'Junio',
    'Julio', 'Agosto', 'Septiembre', 'Octubre', 'Noviembre', 'Diciembre'
  ];

  diasDelMes: any[] = [];
  diaSeleccionado: any = null;
  eventosDelDia: any[] = [];

  constructor(private casoService: CasoService) {}

  ngOnInit() {
    this.cargarRecordatorios();
  }

  cargarRecordatorios() {
    this.casoService.getTodosRecordatorios().subscribe({
      next: (recordatorios) => {
        this.recordatorios = recordatorios;
        this.generarCalendario();
        this.cargando = false;
      },
      error: () => {
        this.error = 'Error al cargar el calendario.';
        this.cargando = false;
      }
    });
  }

  generarCalendario() {
    const primerDia = new Date(this.anioActual, this.mesActual, 1);
    const ultimoDia = new Date(this.anioActual, this.mesActual + 1, 0);
    const diasVacios = primerDia.getDay();

    this.diasDelMes = [];

    // Días vacíos al inicio
    for (let i = 0; i < diasVacios; i++) {
      this.diasDelMes.push(null);
    }

    // Días del mes
    for (let d = 1; d <= ultimoDia.getDate(); d++) {
      const fecha = new Date(this.anioActual, this.mesActual, d);
      const eventos = this.recordatorios.filter(r => {
        const fechaEvento = new Date(r.fechaEnvio);
        return fechaEvento.getDate() === d &&
               fechaEvento.getMonth() === this.mesActual &&
               fechaEvento.getFullYear() === this.anioActual;
      });

      this.diasDelMes.push({
        numero: d,
        fecha,
        eventos,
        esHoy: fecha.toDateString() === this.hoy.toDateString()
      });
    }
  }

  mesAnterior() {
    if (this.mesActual === 0) {
      this.mesActual = 11;
      this.anioActual--;
    } else {
      this.mesActual--;
    }
    this.generarCalendario();
    this.diaSeleccionado = null;
  }

  mesSiguiente() {
    if (this.mesActual === 11) {
      this.mesActual = 0;
      this.anioActual++;
    } else {
      this.mesActual++;
    }
    this.generarCalendario();
    this.diaSeleccionado = null;
  }

  seleccionarDia(dia: any) {
    if (!dia) return;
    this.diaSeleccionado = dia;
    this.eventosDelDia = dia.eventos;
  }

  getBadgeEnviado(enviado: boolean): string {
    return enviado ? 'badge-enviado' : 'badge-pendiente';
  }
}