import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../../shared/navbar/navbar.component';
import { CasoService } from '../../../services/caso.service';
import { environment } from '../../../../environments/environment';
import { CasoSidebarComponent } from './components/caso-sidebar/caso-sidebar.component';
import { AuthService } from '../../../services/auth.service';
import { PanelInfoComponent } from './components/panel-info/panel-info.component';
import { PanelConsultasComponent } from './components/panel-consultas/panel-consultas.component';
import { PanelContenidoComponent } from './components/panel-contenido/panel-contenido.component';
import { PanelArchivosComponent } from './components/panel-archivos/panel-archivos.component';
import { PanelPruebasComponent } from './components/panel-pruebas/panel-pruebas.component';
import { PanelRecordatoriosComponent } from './components/panel-recordatorios/panel-recordatorios.component';
import { PanelEconomiaComponent } from './components/panel-economia/panel-economia.component';

@Component({
  selector: 'app-panel-causas-admin',
  standalone: true,
  imports: [
    CommonModule, FormsModule, NavbarComponent, CasoSidebarComponent,
    PanelInfoComponent, PanelConsultasComponent, PanelContenidoComponent,
    PanelArchivosComponent, PanelPruebasComponent, PanelRecordatoriosComponent,
    PanelEconomiaComponent
  ],
  templateUrl: './panel-causas-admin.component.html',
  styleUrl: './panel-causas-admin.component.scss'
})
export class PanelCausasAdminComponent implements OnInit {
  caso: any = null;
  archivos: any[] = [];
  pruebas: any[] = [];
  recordatorios: any[] = [];
  movimientos: any[] = [];
  resumenEconomico: any = null;
  secciones: any[] = [];
  abogados: any[] = [];
  cargando = true;
  error = '';
  exito = '';
  seccionActiva = 'info';
  consultasPendientesCount = 0;
  apiBase = environment.apiUrl.replace('/api', '');
  menuCasoAbierto = false;

  constructor(
    private route: ActivatedRoute,
    private casoService: CasoService,
    public authService: AuthService
  ) {}

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    const seccion = this.route.snapshot.queryParamMap.get('seccion');
    if (seccion) this.seccionActiva = seccion;

    this.cargarCaso(id);
    this.cargarArchivos(id);
    this.cargarPruebas(id);
    this.cargarRecordatorios(id);
    this.cargarAbogados();
    this.cargarSecciones(id);
    this.cargarMovimientos(id);
  }

  cargarCaso(id: number) {
    this.casoService.getCasoPorId(id).subscribe({
      next: (caso) => {
        this.caso = caso;
        this.cargando = false;

        const comentariosCliente = (caso.comentarios ?? []).filter(
          (c: any) => c.tipoAutor === 'Cliente'
        );
        this.consultasPendientesCount = comentariosCliente.filter((c: any) => {
          const tieneRespuesta = (caso.comentarios ?? []).some(
            (r: any) => r.tipoAutor === 'Abogado' && new Date(r.fecha) > new Date(c.fecha)
          );
          return !tieneRespuesta;
        }).length;
      },
      error: () => { this.error = 'Error al cargar el caso.'; this.cargando = false; }
    });
  }

  cargarArchivos(id: number) {
    this.casoService.getArchivosDeCaso(id).subscribe({
      next: (archivos) => this.archivos = archivos,
      error: () => {}
    });
  }

  cargarPruebas(id: number) {
    this.casoService.getPruebasDeCaso(id).subscribe({
      next: (pruebas) => this.pruebas = pruebas,
      error: () => {}
    });
  }

  cargarRecordatorios(id: number) {
    this.casoService.getRecordatoriosDeCaso(id).subscribe({
      next: (recordatorios) => this.recordatorios = recordatorios,
      error: () => {}
    });
  }

  cargarAbogados() {
    this.casoService.getAbogados().subscribe({
      next: (abogados) => this.abogados = abogados,
      error: () => {}
    });
  }

  cargarSecciones(id: number) {
    this.casoService.getSeccionesDeCaso(id).subscribe({
      next: (secciones) => this.secciones = secciones,
      error: () => {}
    });
  }

  cargarMovimientos(id: number) {
    this.casoService.getMovimientosDeCaso(id).subscribe({
      next: (data) => {
        this.movimientos = data.movimientos ?? data;
        this.resumenEconomico = data.resumen ?? null;
      },
      error: () => {}
    });
  }

  mostrarExito(mensaje: string) {
    this.exito = mensaje;
    setTimeout(() => this.exito = '', 3000);
  }

  mostrarError(mensaje: string) {
    this.error = mensaje;
    setTimeout(() => this.error = '', 3000);
  }
}