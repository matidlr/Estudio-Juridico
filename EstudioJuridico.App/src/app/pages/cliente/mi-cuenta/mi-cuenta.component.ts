import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ClienteSidebarComponent } from '../../../shared/cliente-sidebar/cliente-sidebar.component';
import { CasoService } from '../../../services/caso.service';

@Component({
  selector: 'app-mi-cuenta',
  standalone: true,
  imports: [CommonModule, ClienteSidebarComponent],
  templateUrl: './mi-cuenta.component.html',
  styleUrl: './mi-cuenta.component.scss'
})
export class MiCuentaComponent implements OnInit {
  movimientos: any[] = [];
  movimientosFiltrados: any[] = [];
  resumen: any = null;
  cargando = true;
  error = '';
  filtroTipo = 'todos';

  constructor(private casoService: CasoService) {}

  ngOnInit() {
    this.casoService.getMisMovimientos().subscribe({
      next: (data) => {
        this.movimientos = data.movimientos;
        this.movimientosFiltrados = data.movimientos;
        this.resumen = data.resumen;
        this.cargando = false;
      },
      error: () => {
        this.error = 'Error al cargar los movimientos.';
        this.cargando = false;
      }
    });
  }

  filtrar() {
    if (this.filtroTipo === 'todos') {
      this.movimientosFiltrados = this.movimientos;
    } else {
      this.movimientosFiltrados = this.movimientos.filter(
        m => m.tipo === this.filtroTipo
      );
    }
  }
}