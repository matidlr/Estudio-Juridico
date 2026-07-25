import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CasoService } from '../../../../../services/caso.service';
import { AuthService } from '../../../../../services/auth.service';

@Component({
  selector: 'app-panel-archivos',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './panel-archivos.component.html',
  styleUrl: './panel-archivos.component.scss'
})
export class PanelArchivosComponent {
  @Input() caso: any;
  @Input() archivos: any[] = [];
  @Input() secciones: any[] = [];
  @Output() archivosActualizados = new EventEmitter<void>();
  @Output() mensajeExito = new EventEmitter<string>();
  @Output() mensajeError = new EventEmitter<string>();

  archivoSeleccionado: File | null = null;
  categoriaArchivo = 'Documento';
  seccionArchivo: number | null = null;
  subiendoArchivo = false;

  constructor(public casoService: CasoService, public authService: AuthService) {}

  getIconoArchivo(tipo: string): string {
    if (tipo === 'PDF') return '📄';
    if (tipo === 'JPG' || tipo === 'JPEG' || tipo === 'PNG') return '🖼️';
    if (tipo === 'DOCX') return '📝';
    return '📎';
  }

  getNombreSeccion(seccionId: number): string {
    const seccion = this.secciones.find(s => s.id === seccionId);
    return seccion ? seccion.titulo : '';
  }

  onArchivoChange(event: any) {
    this.archivoSeleccionado = event.target.files[0];
  }

  subirArchivo() {
    if (!this.archivoSeleccionado) return;
    this.subiendoArchivo = true;

    this.casoService.subirArchivo(
      this.caso.id, this.categoriaArchivo,
      this.archivoSeleccionado, this.seccionArchivo ?? undefined
    ).subscribe({
      next: () => {
        this.mensajeExito.emit('Archivo subido correctamente.');
        this.archivoSeleccionado = null;
        this.seccionArchivo = null;
        this.subiendoArchivo = false;
        this.archivosActualizados.emit();
      },
      error: () => {
        this.mensajeError.emit('Error al subir el archivo.');
        this.subiendoArchivo = false;
      }
    });
  }

  eliminarArchivo(id: number) {
    if (!confirm('¿Eliminar este archivo?')) return;
    this.casoService.eliminarArchivo(id).subscribe({
      next: () => {
        this.mensajeExito.emit('Archivo eliminado correctamente.');
        this.archivosActualizados.emit();
      },
      error: () => this.mensajeError.emit('Error al eliminar el archivo.')
    });
  }
}