import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../../shared/navbar/navbar.component';
import { CasoService } from '../../../services/caso.service';
import { environment } from '../../../../environments/environment';
import { CasoSidebarComponent } from './components/caso-sidebar/caso-sidebar.component';
import { AuthService } from '../../../services/auth.service';


@Component({
  selector: 'app-detalle-caso-admin',
  standalone: true,
 imports: [CommonModule, FormsModule, RouterLink, NavbarComponent, CasoSidebarComponent],
  templateUrl: './detalle-caso-admin.component.html',
  styleUrl: './detalle-caso-admin.component.scss'
})
export class DetalleCasoAdminComponent implements OnInit {
  caso: any = null;
  archivos: any[] = [];
  pruebas: any[] = [];
  cargando = true;
  error = '';
  exito = '';
  seccionActiva = 'info';
  apiBase = environment.apiUrl.replace('/api', '');

  // Actualización
  nuevaActualizacion = '';
  enviandoActualizacion = false;

  // Archivo
  archivoSeleccionado: File | null = null;
  categoriaArchivo = 'Documento';
  subiendoArchivo = false;

  // Prueba
  archivoPrueba: File | null = null;
  descripcionPrueba = '';
  subiendoPrueba = false;

  // Recordatorios
recordatorios: any[] = [];
nuevoRecordatorio = {
  titulo: '',
  mensaje: '',
  fechaEnvio: '',
  tipo: 'Recordatorio',
  casoId: 0
};
creandoRecordatorio = false;

  // Editar caso
  editando = false;
  tipos = ['Laboral', 'Civil', 'Penal', 'Familia', 'Comercial'];
  etapas = [
    'Consulta inicial', 'Mediación', 'Demanda presentada',
    'Instrucción / prueba', 'Audiencia', 'Alegatos',
    'Sentencia', 'Apelación', 'Resolución final'
  ];
  estados = ['Activo', 'Suspendido', 'Finalizado', 'Archivado'];
  abogados: any[] = [];
  reasignando = false;
  abogadoSeleccionado = 0;

  nroFoja = '';
  aclaracionCliente = '';
  busquedaFoja = '';
  actualizacionesFiltradas: any[] = [];
  seccionFoja: any = null;

  mostrarFormFoja = false;
  mostrarFormSeccion = false;

  nuevaSeccion = {
  titulo: '',
  descripcion: '',
  fojaDesde: 0,
  fojaHasta: 0,
  orden: 0
};
secciones: any[] = [];
  

  constructor(
    private route: ActivatedRoute,
    private casoService: CasoService,
    public authService: AuthService 
  ) {}

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.cargarCaso(id);
    this.cargarArchivos(id);
    this.cargarPruebas(id);
    this.cargarRecordatorios(id);
    this.cargarAbogados();
    this.cargarSecciones(id);
  }

  cargarCaso(id: number) {
  this.casoService.getCasoPorId(id).subscribe({
    next: (caso) => {
      this.caso = caso;
      this.actualizacionesFiltradas = caso.actualizaciones ?? [];
      this.cargando = false;
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

  agregarActualizacion() {
  if (!this.nuevaActualizacion.trim()) return;
  this.enviandoActualizacion = true;

  this.casoService.agregarActualizacion({
    contenido:           this.nuevaActualizacion,
    casoId:              this.caso.id,
    nroFoja:             this.nroFoja,
    aclaracionCliente:   this.aclaracionCliente,
    seccionExpedienteId: this.seccionFoja
  }).subscribe({
    next: () => {
      this.exito = 'Foja agregada correctamente.';
      this.nuevaActualizacion = '';
      this.nroFoja = '';
      this.aclaracionCliente = '';
      this.seccionFoja = null;
      this.enviandoActualizacion = false;
      this.mostrarFormFoja = false;
      this.cargarCaso(this.caso.id);
      setTimeout(() => this.exito = '', 3000);
    },
    error: () => {
      this.error = 'Error al agregar la foja.';
      this.enviandoActualizacion = false;
    }
  });
}

  onArchivoChange(event: any) {
    this.archivoSeleccionado = event.target.files[0];
  }

  onArchivoPruebaChange(event: any) {
    this.archivoPrueba = event.target.files[0];
  }

  subirArchivo() {
    if (!this.archivoSeleccionado) return;
    this.subiendoArchivo = true;

    this.casoService.subirArchivo(
      this.caso.id,
      this.categoriaArchivo,
      this.archivoSeleccionado
    ).subscribe({
      next: () => {
        this.exito = 'Archivo subido correctamente.';
        this.archivoSeleccionado = null;
        this.subiendoArchivo = false;
        this.cargarArchivos(this.caso.id);
        setTimeout(() => this.exito = '', 3000);
      },
      error: () => {
        this.error = 'Error al subir el archivo.';
        this.subiendoArchivo = false;
      }
    });
  }

  subirPrueba() {
    if (!this.archivoPrueba || !this.descripcionPrueba.trim()) return;
    this.subiendoPrueba = true;

    this.casoService.subirPrueba(
      this.caso.id,
      this.descripcionPrueba,
      this.archivoPrueba
    ).subscribe({
      next: () => {
        this.exito = 'Prueba agregada correctamente.';
        this.archivoPrueba = null;
        this.descripcionPrueba = '';
        this.subiendoPrueba = false;
        this.cargarPruebas(this.caso.id);
        setTimeout(() => this.exito = '', 3000);
      },
      error: () => {
        this.error = 'Error al subir la prueba.';
        this.subiendoPrueba = false;
      }
    });
  }

  eliminarArchivo(id: number) {
    if (!confirm('¿Seguro que querés eliminar este archivo?')) return;
    this.casoService.eliminarArchivo(id).subscribe({
      next: () => this.cargarArchivos(this.caso.id),
      error: () => this.error = 'Error al eliminar el archivo.'
    });
  }

  eliminarPrueba(id: number) {
    if (!confirm('¿Seguro que querés eliminar esta prueba?')) return;
    this.casoService.eliminarPrueba(id).subscribe({
      next: () => this.cargarPruebas(this.caso.id),
      error: () => this.error = 'Error al eliminar la prueba.'
    });
  }

  guardarCambios() {
    this.casoService.editarCaso(this.caso.id, {
      titulo: this.caso.titulo,
      nombrePartes: this.caso.nombrePartes,
      descripcion: this.caso.descripcion,
      tipo: this.caso.tipo,
      estado: this.caso.estado,
      etapa: this.caso.etapa,
      clienteId: this.caso.clienteId
    }).subscribe({
      next: () => {
        this.exito = 'Caso actualizado correctamente.';
        this.editando = false;
        setTimeout(() => this.exito = '', 3000);
      },
      error: () => this.error = 'Error al guardar los cambios.'
    });
  }

  getIconoArchivo(tipo: string): string {
    const iconos: any = {
      'PDF': '📄', 'JPG': '🖼️', 'JPEG': '🖼️',
      'PNG': '🖼️', 'TXT': '📝', 'DOCX': '📝'
    };
    return iconos[tipo] ?? '📎';
  }

  cargarRecordatorios(id: number) {
  this.casoService.getRecordatoriosDeCaso(id).subscribe({
    next: (recordatorios) => this.recordatorios = recordatorios,
    error: () => {}
  });
}

crearRecordatorio() {
  if (!this.nuevoRecordatorio.titulo || !this.nuevoRecordatorio.fechaEnvio) return;
  this.creandoRecordatorio = true;

  this.casoService.crearRecordatorio({
    ...this.nuevoRecordatorio,
    casoId: this.caso.id
  }).subscribe({
    next: () => {
      this.exito = this.nuevoRecordatorio.tipo === 'Vencimiento'
        ? 'Vencimiento agregado correctamente.'
        : 'Recordatorio creado. Se enviará en la fecha indicada.';
      this.nuevoRecordatorio = { titulo: '', mensaje: '', fechaEnvio: '', tipo: 'Recordatorio', casoId: 0 };
      this.creandoRecordatorio = false;
      this.cargarRecordatorios(this.caso.id);
      setTimeout(() => this.exito = '', 3000);
    },
    error: () => {
      this.error = 'Error al crear el recordatorio.';
      this.creandoRecordatorio = false;
    }
  });
}
eliminarRecordatorio(id: number) {
  if (!confirm('¿Seguro que querés eliminar este recordatorio?')) return;
  this.casoService.eliminarRecordatorio(id).subscribe({
    next: () => this.cargarRecordatorios(this.caso.id),
    error: () => this.error = 'Error al eliminar el recordatorio.'
  });
}
cargarAbogados() {
  this.casoService.getAbogados().subscribe({
    next: (abogados) => {
      this.abogados = abogados;
      console.log('Abogados cargados:', abogados); // agregás este log
    },
    error: (err) => {
      console.log('Error al cargar abogados:', err); // y este
    }
  });
}
reasignarAbogado() {
  if (!this.abogadoSeleccionado) return;
  this.reasignando = true;

  this.casoService.reasignarAbogado(this.caso.id, this.abogadoSeleccionado).subscribe({
    next: () => {
      this.exito = 'Abogado reasignado correctamente.';
      this.reasignando = false;
      this.cargarCaso(this.caso.id);
      setTimeout(() => this.exito = '', 3000);
    },
    error: () => {
      this.error = 'Error al reasignar el abogado.';
      this.reasignando = false;
    }
  });
}

filtrarFojas() {
  if (!this.busquedaFoja.trim()) {
    this.actualizacionesFiltradas = this.caso.actualizaciones ?? [];
    return;
  }
  this.actualizacionesFiltradas = (this.caso.actualizaciones ?? []).filter((a: any) =>
    a.nroFoja?.toLowerCase().includes(this.busquedaFoja.toLowerCase()) ||
    a.contenido?.toLowerCase().includes(this.busquedaFoja.toLowerCase())
  );
}


cargarSecciones(id: number) {
  this.casoService.getSeccionesDeCaso(id).subscribe({
    next: (secciones) => this.secciones = secciones,
    error: () => {}
  });
}

crearSeccion() {
  if (!this.nuevaSeccion.titulo) return;

  this.casoService.crearSeccion({
    ...this.nuevaSeccion,
    casoId: this.caso.id
  }).subscribe({
    next: () => {
      this.exito = 'Sección creada correctamente.';
      this.nuevaSeccion = { titulo: '', descripcion: '', fojaDesde: 0, fojaHasta: 0, orden: 0 };
      this.mostrarFormSeccion = false;
      this.cargarSecciones(this.caso.id);
      setTimeout(() => this.exito = '', 3000);
    },
    error: () => this.error = 'Error al crear la sección.'
  });
}
}