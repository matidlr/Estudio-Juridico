import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminSidebarComponent } from '../../../shared/admin-sidebar/admin-sidebar.component';
import { CasoService } from '../../../services/caso.service';

@Component({
  selector: 'app-consultas-nuevos-clientes',
  standalone: true,
  imports: [CommonModule, AdminSidebarComponent],
  templateUrl: './consultas-nuevos-clientes.component.html',
  styleUrl: './consultas-nuevos-clientes.component.scss'
})
export class ConsultasNuevosClientesComponent implements OnInit {
  consultas: any[] = [];
  cargando = true;
  error = '';

  constructor(private casoService: CasoService) {}

  ngOnInit() {
    this.cargar();
  }

  cargar() {
    this.casoService.getConsultasPublicas().subscribe({
      next: (data) => {
        this.consultas = data;
        this.cargando = false;
      },
      error: () => {
        this.error = 'Error al cargar las consultas.';
        this.cargando = false;
      }
    });
  }

  marcarAtendida(id: number) {
    this.casoService.marcarConsultaAtendida(id).subscribe({
      next: () => {
        const consulta = this.consultas.find(c => c.id === id);
        if (consulta) consulta.atendida = !consulta.atendida;
      },
      error: () => {}
    });
  }

  eliminar(id: number) {
    if (!confirm('¿Eliminar esta consulta?')) return;
    this.casoService.eliminarConsultaPublica(id).subscribe({
      next: () => {
        this.consultas = this.consultas.filter(c => c.id !== id);
      },
      error: () => {}
    });
  }

  getPendientes(): number {
    return this.consultas.filter(c => !c.atendida).length;
  }
}