import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CasoService } from '../../../../../services/caso.service';
import { environment } from '../../../../../../environments/environment';

@Component({
  selector: 'app-panel-pruebas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './panel-pruebas.component.html',
  styleUrl: './panel-pruebas.component.scss'
})
export class PanelPruebasComponent {
  @Input() caso: any;
  @Input() pruebas: any[] = [];
  @Input() secciones: any[] = [];
  @Output() pruebasActualizadas = new EventEmitter<void>();
  @Output() mensajeExito = new EventEmitter<string>();
  @Output() mensajeError = new EventEmitter<string>();

  archivoPrueba: File | null = null;
  descripcionPrueba = '';
  seccionPrueba: number | null = null;
  subiendoPrueba = false;
  apiBase = environment.apiUrl.replace('/api', '');

  constructor(private casoService: CasoService) {}

  getIconoArchivo(tipo: string): string {
    if (tipo === 'PDF') return '📄';
    if (tipo === 'JPG' || tipo === 'JPEG' || tipo === 'PNG') return '🖼️';
    if (tipo === 'DOCX') return '📝';
    return '📎';
  }

  onArchivoPruebaChange(event: any) {
    this.archivoPrueba = event.target.files[0];
  }

  subirPrueba() {
    if (!this.archivoPrueba || !this.descripcionPrueba) return;
    this.subiendoPrueba = true;

    this.casoService.subirPrueba(
      this.caso.id, this.descripcionPrueba,
      this.archivoPrueba, this.seccionPrueba ?? undefined
    ).subscribe({
      next: () => {
        this.mensajeExito.emit('Prueba subida correctamente.');
        this.archivoPrueba = null;
        this.descripcionPrueba = '';
        this.subiendoPrueba = false;
        this.pruebasActualizadas.emit();
      },
      error: () => {
        this.mensajeError.emit('Error al subir la prueba.');
        this.subiendoPrueba = false;
      }
    });
  }

  eliminarPrueba(id: number) {
    if (!confirm('¿Eliminar esta prueba?')) return;
    this.casoService.eliminarPrueba(id).subscribe({
      next: () => {
        this.mensajeExito.emit('Prueba eliminada correctamente.');
        this.pruebasActualizadas.emit();
      },
      error: () => this.mensajeError.emit('Error al eliminar la prueba.')
    });
  }
}