import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AdminSidebarComponent } from '../../../shared/admin-sidebar/admin-sidebar.component';
import { CasoService } from '../../../services/caso.service';

@Component({
  selector: 'app-estadisticas',
  standalone: true,
  imports: [CommonModule, AdminSidebarComponent],
  templateUrl: './estadisticas.component.html',
  styleUrl: './estadisticas.component.scss'
})
export class EstadisticasComponent implements OnInit {
  datos: any = null;
  cargando = true;
  error = '';
  filtroMeses = 1;

  meses = [
    { valor: 1, label: 'Último mes' },
    { valor: 3, label: 'Últimos 3 meses' },
    { valor: 6, label: 'Últimos 6 meses' },
    { valor: 12, label: 'Último año' },
    { valor: 36, label: 'Últimos 3 años' }
  ];

  constructor(private casoService: CasoService) {}

  ngOnInit() {
    this.cargar();
  }

  cargar() {
    this.cargando = true;
    this.casoService.getEstadisticas(this.filtroMeses).subscribe({
      next: (datos) => {
        this.datos = datos;
        this.cargando = false;
      },
      error: () => {
        this.error = 'Error al cargar las estadísticas.';
        this.cargando = false;
      }
    });
  }

  cambiarFiltro(meses: number) {
    this.filtroMeses = meses;
    this.cargar();
  }

  getNombreMes(mes: number): string {
    const meses = ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun',
                   'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'];
    return meses[mes - 1];
  }

  getMaxCausasPorMes(): number {
    if (!this.datos?.causasPorMes?.length) return 1;
    return Math.max(...this.datos.causasPorMes.map((c: any) => c.cantidad));
  }
}