import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../../shared/navbar/navbar.component';
import { CasoService } from '../../../services/caso.service';

@Component({
  selector: 'app-panel-cliente',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, NavbarComponent],
  templateUrl: './panel-cliente.component.html',
  styleUrl: './panel-cliente.component.scss'
})
export class PanelClienteComponent implements OnInit {
  casos: any[] = [];
  casosFiltrados: any[] = [];
  busqueda = '';
  filtroEstado = 'todos';
  filtroTipo = 'todos';
  cargando = true;
  error = '';

  constructor(private casoService: CasoService, private router: Router) {}

  ngOnInit() {
    this.casoService.getMisCasos().subscribe({
      next: (casos) => {
        this.casos = casos;
        this.casosFiltrados = casos;
        this.cargando = false;
      },
      error: () => {
        this.error = 'Error al cargar los casos.';
        this.cargando = false;
      }
    });
  }

  filtrar() {
    this.casosFiltrados = this.casos.filter(c => {
      const matchBusqueda = this.busqueda === '' ||
        c.titulo.toLowerCase().includes(this.busqueda.toLowerCase()) ||
        c.nombrePartes.toLowerCase().includes(this.busqueda.toLowerCase());

      const matchEstado = this.filtroEstado === 'todos' ||
        c.estado.toLowerCase() === this.filtroEstado;

      const matchTipo = this.filtroTipo === 'todos' ||
        c.tipo.toLowerCase() === this.filtroTipo;

      return matchBusqueda && matchEstado && matchTipo;
    });
  }

  verDetalle(id: number) {
    this.router.navigate(['/cliente/caso', id]);
  }

  getBadgeEstado(estado: string): string {
    const badges: any = {
      'Activo': 'badge-activo',
      'Suspendido': 'badge-suspendido',
      'Finalizado': 'badge-cerrado',
      'Archivado': 'badge-cerrado'
    };
    return badges[estado] ?? 'badge-cerrado';
  }

  getBadgeTipo(tipo: string): string {
    const badges: any = {
      'Laboral': 'badge-laboral',
      'Civil': 'badge-civil',
      'Penal': 'badge-penal',
      'Familia': 'badge-familia',
      'Comercial': 'badge-comercial'
    };
    return badges[tipo] ?? '';
  }
}