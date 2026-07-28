import { Component, Input, Output, EventEmitter, OnInit, OnChanges } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CasoService } from '../../../../../services/caso.service';
import { environment } from '../../../../../../environments/environment';
import { PdfService, PdfOpciones, PDF_OPCIONES_DEFAULT } from '../../../../../services/pdf.service';

@Component({
  selector: 'app-panel-contenido',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './panel-contenido.component.html',
  styleUrl: './panel-contenido.component.scss'
})
export class PanelContenidoComponent implements OnInit {
  @Input() caso: any;
  @Input() secciones: any[] = [];
  @Output() casoActualizado = new EventEmitter<void>();
  @Output() mensajeExito = new EventEmitter<string>();
  @Output() mensajeError = new EventEmitter<string>();

  apiBase = environment.apiUrl.replace('/api', '');

  fojaActual: any = null;
  paginaActual = 1;
  totalFojas = 0;
  cargandoFojas = false;
  fojaContenidoEditado = '';
  fojaAclaracionEditada = '';
  fojaModificada = false;
  guardandoFoja = false;
  mostrarOpcionesCopiar = false;
  seccionSeleccionada: any = null;
  busquedaFoja = '';
  mostrarFormFoja = false;
  mostrarFormSeccion = false;
  nuevaActualizacion = '';
  nroFoja = '';
  aclaracionCliente = '';
  seccionFoja: any = null;
  enviandoActualizacion = false;
  nuevaSeccion = { titulo: '', descripcion: '', fojaDesde: 0, fojaHasta: 0, orden: 0 };
  versiones: any[] = [];
  mostrandoVersiones = false;
  cargandoVersiones = false;
  archivosFoja: any[] = [];
  archivoFojaSeleccionado: File | null = null;
  subiendoArchivoFoja = false;
  archivosSeccion: any[] = [];
  pruebasSeccion: any[] = [];

  mostrarModalPdf = false;
opcionesPdf: PdfOpciones = PDF_OPCIONES_DEFAULT;

  constructor(public casoService: CasoService, private pdfService: PdfService) {}

  ngOnInit() {
    this.cargarFojas(1);
    this.opcionesPdf = this.pdfService.cargarOpciones();
  }

  getIconoArchivo(tipo: string): string {
    if (tipo === 'PDF') return '📄';
    if (tipo === 'JPG' || tipo === 'JPEG' || tipo === 'PNG') return '🖼️';
    if (tipo === 'DOCX') return '📝';
    return '📎';
  }

  cargarFojas(pagina: number = 1) {
    this.cargandoFojas = true;
    this.paginaActual = pagina;
    this.fojaModificada = false;

    const seccionId = this.seccionSeleccionada?.id;
    const busqueda = this.busquedaFoja || undefined;

    this.casoService.getFojasPaginadas(this.caso.id, pagina, 1, seccionId, busqueda).subscribe({
      next: (data) => {
        this.fojaActual = data.foja;
        this.totalFojas = data.total;
        this.archivosSeccion = data.archivosSeccion ?? [];
        this.pruebasSeccion = data.pruebasSeccion ?? [];
        this.fojaContenidoEditado = data.foja?.contenido ?? '';
        this.fojaAclaracionEditada = data.foja?.aclaracionCliente ?? '';
        this.fojaModificada = false;
        this.cargandoFojas = false;
        if (data.foja) this.cargarArchivosFoja(data.foja.id);
      },
      error: () => { this.cargandoFojas = false; }
    });
  }

  filtrarFojas() {
    this.cargarFojas(1);
  }

  getPaginas(): number[] {
    const rango = 5;
    const paginas: number[] = [];
    let inicio = Math.max(1, this.paginaActual - rango);
    let fin = Math.min(this.totalFojas, this.paginaActual + rango);
    for (let i = inicio; i <= fin; i++) paginas.push(i);
    return paginas;
  }

  agregarActualizacion() {
  if (!this.nuevaActualizacion.trim()) return;

  if (this.nuevaActualizacion.length > 50000) {
    this.mensajeError.emit('El contenido no puede superar los 50.000 caracteres.');
    return;
  }

  this.enviandoActualizacion = true;

  this.casoService.agregarActualizacion({
    contenido: this.nuevaActualizacion,
    casoId: this.caso.id,
    nroFoja: this.nroFoja,
    aclaracionCliente: this.aclaracionCliente,
    seccionExpedienteId: this.seccionFoja
  }).subscribe({
    next: () => {
      this.mensajeExito.emit('Foja agregada correctamente.');
      this.nuevaActualizacion = '';
      this.nroFoja = '';
      this.aclaracionCliente = '';
      this.seccionFoja = null;
      this.enviandoActualizacion = false;
      this.mostrarFormFoja = false;
      this.cargarFojas(1);
    },
    error: (err) => {
      this.mensajeError.emit(err.error?.mensaje ?? 'Error al agregar la foja.');
      this.enviandoActualizacion = false;
    }
  });
}

  guardarEdicionInline() {
    if (!this.fojaActual) return;
    this.guardandoFoja = true;

    this.casoService.editarActualizacion(this.fojaActual.id, {
      contenido: this.fojaContenidoEditado,
      nroFoja: this.fojaActual.nroFoja,
      aclaracionCliente: this.fojaAclaracionEditada,
      casoId: this.caso.id
    }).subscribe({
      next: () => {
        this.mensajeExito.emit('Foja guardada correctamente.');
        this.fojaModificada = false;
        this.guardandoFoja = false;
        this.cargarFojas(this.paginaActual);
      },
      error: (err) => {
        this.mensajeError.emit(err.error?.mensaje ?? 'Error al guardar la foja.');
        this.guardandoFoja = false;
      }
    });
  }

  descartarCambios() {
    this.fojaContenidoEditado = this.fojaActual?.contenido ?? '';
    this.fojaAclaracionEditada = this.fojaActual?.aclaracionCliente ?? '';
    this.fojaModificada = false;
  }

  eliminarFoja(id: number) {
    if (!confirm('¿Seguro que querés eliminar esta foja?')) return;
    this.casoService.eliminarActualizacion(id).subscribe({
      next: () => {
        this.mensajeExito.emit('Foja eliminada correctamente.');
        this.cargarFojas(1);
      },
      error: () => this.mensajeError.emit('Error al eliminar la foja.')
    });
  }

  crearSeccion() {
    this.casoService.crearSeccion({ ...this.nuevaSeccion, casoId: this.caso.id }).subscribe({
      next: () => {
        this.mensajeExito.emit('Sección creada correctamente.');
        this.nuevaSeccion = { titulo: '', descripcion: '', fojaDesde: 0, fojaHasta: 0, orden: 0 };
        this.mostrarFormSeccion = false;
        this.casoActualizado.emit();
      },
      error: () => this.mensajeError.emit('Error al crear la sección.')
    });
  }

  eliminarSeccion(id: number, event: Event) {
    event.stopPropagation();
    if (!confirm('¿Eliminar esta sección? Las fojas no se eliminarán.')) return;
    this.casoService.eliminarSeccion(id).subscribe({
      next: () => {
        this.mensajeExito.emit('Sección eliminada correctamente.');
        this.seccionSeleccionada = null;
        this.casoActualizado.emit();
        this.cargarFojas(1);
      },
      error: () => this.mensajeError.emit('Error al eliminar la sección.')
    });
  }

  verVersiones(fojaId: number) {
    this.mostrandoVersiones = true;
    this.cargandoVersiones = true;
    this.casoService.getVersionesFoja(fojaId).subscribe({
      next: (data) => { this.versiones = data.datos ?? data; this.cargandoVersiones = false; },
      error: () => this.cargandoVersiones = false
    });
  }

  cerrarVersiones() {
    this.mostrandoVersiones = false;
    this.versiones = [];
  }

  restaurarVersion(fojaId: number, versionId: number) {
    if (!confirm('¿Restaurar esta versión?')) return;
    this.casoService.restaurarVersionFoja(fojaId, versionId).subscribe({
      next: () => {
        this.mensajeExito.emit('Foja restaurada correctamente.');
        this.mostrandoVersiones = false;
        this.versiones = [];
        this.cargarFojas(this.paginaActual);
      },
      error: () => this.mensajeError.emit('Error al restaurar la versión.')
    });
  }

  cargarArchivosFoja(actualizacionId: number) {
    this.casoService.getArchivosDeFoja(actualizacionId).subscribe({
      next: (data) => this.archivosFoja = data.datos ?? data,
      error: () => {}
    });
  }

  onArchivoFojaChange(event: any) {
    this.archivoFojaSeleccionado = event.target.files[0];
  }

  subirArchivoFoja() {
    if (!this.archivoFojaSeleccionado || !this.fojaActual) return;
    this.subiendoArchivoFoja = true;
    this.casoService.subirArchivoFoja(this.caso.id, 'Documento', this.archivoFojaSeleccionado, this.fojaActual.id).subscribe({
      next: () => {
        this.mensajeExito.emit('Archivo adjuntado correctamente.');
        this.archivoFojaSeleccionado = null;
        this.subiendoArchivoFoja = false;
        this.cargarArchivosFoja(this.fojaActual.id);
      },
      error: () => {
        this.mensajeError.emit('Error al subir el archivo.');
        this.subiendoArchivoFoja = false;
      }
    });
  }

  eliminarArchivoFoja(id: number) {
    if (!confirm('¿Eliminar este archivo?')) return;
    this.casoService.eliminarArchivoFoja(id).subscribe({
      next: () => this.cargarArchivosFoja(this.fojaActual.id),
      error: () => this.mensajeError.emit('Error al eliminar el archivo.')
    });
  }

  copiarFoja() {
  const texto = this.fojaContenidoEditado;
  navigator.clipboard.writeText(texto).then(() => {
    this.mensajeExito.emit('Foja copiada al portapapeles.');
  }).catch(() => {
    this.mensajeError.emit('No se pudo copiar al portapapeles.');
  });
}

  copiarSeccion() {
    const seccionId = this.seccionSeleccionada?.id;
    this.casoService.getFojasPaginadas(this.caso.id, 1, 999, seccionId, undefined).subscribe({
      next: (data) => {
        const todasLasFojas = data.fojas ?? [];
        let texto = '';
        if (this.seccionSeleccionada) {
          texto += `SECCIÓN: ${this.seccionSeleccionada.titulo}\n`;
          texto += `Fojas ${this.seccionSeleccionada.fojaDesde} a ${this.seccionSeleccionada.fojaHasta}\n`;
          texto += '='.repeat(50) + '\n\n';
        }
        todasLasFojas.forEach((foja: any) => {
          texto += `Foja ${foja.nroFoja ?? ''}\n`;
          texto += '-'.repeat(30) + '\n';
          texto += foja.contenido + '\n';
          if (foja.aclaracionCliente) texto += `\nAclaración: ${foja.aclaracionCliente}\n`;
          texto += '\n';
        });
        navigator.clipboard.writeText(texto).then(() => {
          this.mensajeExito.emit('Sección copiada al portapapeles.');
        }).catch(() => {
          this.mensajeError.emit('No se pudo copiar al portapapeles.');
        });
      },
      error: () => this.mensajeError.emit('Error al copiar la sección.')
    });
  }

  guardarOpcionesDefault() {
  this.pdfService.guardarOpciones(this.opcionesPdf);
  this.mensajeExito.emit('Configuración guardada como predeterminada.');
}

exportarPDF() {
  this.mostrarModalPdf = false;
  this.cargandoFojas = true;
  const seccionId = this.seccionSeleccionada?.id;

  this.casoService.getFojasParaExportar(this.caso.id, seccionId).subscribe({
    next: (data) => {
      const fojas = data.datos?.fojas ?? data.fojas ?? [];
      this.pdfService.exportarExpediente(this.caso, fojas, this.opcionesPdf);
      this.cargandoFojas = false;
    },
    error: () => {
      this.mensajeError.emit('Error al exportar el expediente.');
      this.cargandoFojas = false;
    }
  });
}

}