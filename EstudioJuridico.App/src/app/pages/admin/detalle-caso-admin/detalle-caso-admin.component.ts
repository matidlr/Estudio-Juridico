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
  seccionActiva: string = 'info';
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
seccionSeleccionada: any = null;

// Consultas
respuesta = '';
enviandoRespuesta = false;

// Economía
movimientos: any[] = [];
resumenEconomico: any = null;
nuevoMovimiento = {
  tipo: 'Honorario',
  concepto: '',
  monto: 0,
  fecha: new Date().toISOString().split('T')[0],
  formaPago: '',
  notas: ''
};
guardandoMovimiento = false;
  
consultasPendientesCount = 0;

// Paginación de fojas
fojaActual: any = null;
paginaActual = 1;
totalFojas = 0;
cargandoFojas = false;
fojasExpandidas: Set<number> = new Set();

// Editar foja
fojaEditando: any = null;
fojaEditandoContenido = '';
fojaEditandoNroFoja = '';
fojaEditandoAclaracion = '';
guardandoFoja = false;


  constructor(
    private route: ActivatedRoute,
    private casoService: CasoService,
    public authService: AuthService 
  ) {}

  ngOnInit() {
  const id = Number(this.route.snapshot.paramMap.get('id'));
  const seccion = this.route.snapshot.queryParamMap.get('seccion');
  if (seccion) {
    this.seccionActiva = seccion;
  }
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
      this.actualizacionesFiltradas = caso.actualizaciones ?? [];
      this.cargando = false;
       this.cargarFojas(1); //

      // Calculamos consultas pendientes
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
    caratula:      this.caso.caratula,
    proceso:       this.caso.proceso,
    juzgado:       this.caso.juzgado,
    nroExpediente: this.caso.nroExpediente,
    tipo:          this.caso.tipo,
    estado:        this.caso.estado,
    etapa:         this.caso.etapa,
    clienteId:     this.caso.clienteId
  }).subscribe({
    next: () => {
      this.exito = 'Causa actualizada correctamente.';
      this.editando = false;
      this.cargarCaso(this.caso.id);
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
  this.cargarFojas(1);
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

responderConsulta() {
  if (!this.respuesta.trim()) return;
  this.enviandoRespuesta = true;

  this.casoService.responderComentario(this.caso.id, this.respuesta).subscribe({
    next: () => {
      this.exito = 'Respuesta enviada correctamente.';
      this.respuesta = '';
      this.enviandoRespuesta = false;
      this.cargarCaso(this.caso.id);
      setTimeout(() => this.exito = '', 3000);
    },
    error: () => {
      this.error = 'Error al enviar la respuesta.';
      this.enviandoRespuesta = false;
    }
  });
}

cargarMovimientos(id: number) {
  this.casoService.getMovimientosDeCaso(id).subscribe({
    next: (data) => {
      this.movimientos = data.movimientos;
      this.resumenEconomico = data.resumen;
    },
    error: () => {}
  });
}

crearMovimiento() {
  if (!this.nuevoMovimiento.concepto || !this.nuevoMovimiento.monto) return;
  this.guardandoMovimiento = true;

  this.casoService.crearMovimiento({
    ...this.nuevoMovimiento,
    casoId: this.caso.id
  }).subscribe({
    next: () => {
      this.exito = 'Movimiento registrado correctamente.';
      this.nuevoMovimiento = {
        tipo: 'Honorario',
        concepto: '',
        monto: 0,
        fecha: new Date().toISOString().split('T')[0],
        formaPago: '',
        notas: ''
      };
      this.guardandoMovimiento = false;
      this.cargarMovimientos(this.caso.id);
      setTimeout(() => this.exito = '', 3000);
    },
    error: () => {
      this.error = 'Error al registrar el movimiento.';
      this.guardandoMovimiento = false;
    }
  });
}

eliminarMovimiento(id: number) {
  if (!confirm('¿Seguro que querés eliminar este movimiento?')) return;
  this.casoService.eliminarMovimiento(id).subscribe({
    next: () => this.cargarMovimientos(this.caso.id),
    error: () => this.error = 'Error al eliminar el movimiento.'
  });
}

getColorMovimiento(tipo: string): string {
  if (tipo === 'Pago') return 'movimiento-pago';
  if (tipo === 'Gasto') return 'movimiento-gasto';
  return 'movimiento-honorario';
}

cargarFojas(pagina: number = 1) {
  this.cargandoFojas = true;
  this.paginaActual = pagina;

  const seccionId = this.seccionSeleccionada?.id;
  const busqueda = this.busquedaFoja || undefined;

  this.casoService.getFojasPaginadas(this.caso.id, pagina, 1, seccionId, busqueda).subscribe({
    next: (data) => {
      this.fojaActual = data.foja;
      this.totalFojas = data.total;
      this.cargandoFojas = false;
    },
    error: () => {
      this.cargandoFojas = false;
    }
  });
}

getPaginas(): number[] {
  return Array.from({ length: this.totalFojas }, (_, i) => i + 1);
}

toggleFoja(id: number) {
  if (this.fojasExpandidas.has(id)) {
    this.fojasExpandidas.delete(id);
  } else {
    this.fojasExpandidas.add(id);
  }
}

estaExpandida(id: number): boolean {
  return this.fojasExpandidas.has(id);
}

editarFoja(foja: any) {
  this.fojaEditando = foja;
  this.fojaEditandoContenido = foja.contenido;
  this.fojaEditandoNroFoja = foja.nroFoja ?? '';
  this.fojaEditandoAclaracion = foja.aclaracionCliente ?? '';
}

cancelarEdicionFoja() {
  this.fojaEditando = null;
  this.fojaEditandoContenido = '';
  this.fojaEditandoNroFoja = '';
  this.fojaEditandoAclaracion = '';
}

guardarEdicionFoja() {
  if (!this.fojaEditandoContenido.trim()) return;
  this.guardandoFoja = true;

  this.casoService.editarActualizacion(this.fojaEditando.id, {
    contenido:         this.fojaEditandoContenido,
    nroFoja:           this.fojaEditandoNroFoja,
    aclaracionCliente: this.fojaEditandoAclaracion,
    casoId:            this.caso.id
  }).subscribe({
    next: () => {
      this.exito = 'Foja actualizada correctamente.';
      this.guardandoFoja = false;
      this.cancelarEdicionFoja();
      this.cargarFojas(1);
      setTimeout(() => this.exito = '', 3000);
    },
    error: (err) => {
      this.error = err.error?.mensaje ?? 'Error al actualizar la foja.';
      this.guardandoFoja = false;
    }
  });
}

eliminarFoja(id: number) {
  if (!confirm('¿Seguro que querés eliminar esta foja? Esta acción no se puede deshacer.')) return;

  this.casoService.eliminarActualizacion(id).subscribe({
    next: () => {
      this.exito = 'Foja eliminada correctamente.';
      this.cargarFojas(1);
      setTimeout(() => this.exito = '', 3000);
    },
    error: () => this.error = 'Error al eliminar la foja.'
  });
}
}