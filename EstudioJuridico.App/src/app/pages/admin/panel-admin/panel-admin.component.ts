import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../../shared/navbar/navbar.component';
import { CasoService } from '../../../services/caso.service';

@Component({
  selector: 'app-panel-admin',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, NavbarComponent],
  templateUrl: './panel-admin.component.html',
  styleUrl: './panel-admin.component.scss'
})
export class PanelAdminComponent implements OnInit {
  casos: any[] = [];
  casosFiltrados: any[] = [];
  busqueda = '';
  filtroEstado = 'todos';
  cargando = true;
  error = '';

  constructor(private casoService: CasoService, private router: Router) {}

  ngOnInit() {
    this.casoService.getTodosCasos().subscribe({
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
        c.titulo.toLowerCase().includes(this.busqueda.toLowerCase());

      const matchEstado = this.filtroEstado === 'todos' ||
        c.estado.toLowerCase() === this.filtroEstado;

      return matchBusqueda && matchEstado;
    });
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

editarCaso(id: number) {
  this.router.navigate(['/admin/caso', id]);
}
}