import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CasoService } from '../../../../../services/caso.service';

@Component({
  selector: 'app-panel-recordatorios',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './panel-recordatorios.component.html',
  styleUrl: './panel-recordatorios.component.scss'
})
export class PanelRecordatoriosComponent {
  @Input() caso: any;
  @Input() recordatorios: any[] = [];
  @Output() recordatoriosActualizados = new EventEmitter<void>();
  @Output() mensajeExito = new EventEmitter<string>();
  @Output() mensajeError = new EventEmitter<string>();

  nuevoRecordatorio = {
    titulo: '', mensaje: '', fechaEnvio: '',
    tipo: 'Recordatorio', casoId: 0
  };
  creandoRecordatorio = false;

  constructor(private casoService: CasoService) {}

  crearRecordatorio() {
    if (!this.nuevoRecordatorio.titulo || !this.nuevoRecordatorio.fechaEnvio) return;
    this.creandoRecordatorio = true;

    this.casoService.crearRecordatorio({
      ...this.nuevoRecordatorio,
      casoId: this.caso.id
    }).subscribe({
      next: () => {
        this.mensajeExito.emit('Recordatorio programado correctamente.');
        this.nuevoRecordatorio = { titulo: '', mensaje: '', fechaEnvio: '', tipo: 'Recordatorio', casoId: 0 };
        this.creandoRecordatorio = false;
        this.recordatoriosActualizados.emit();
      },
      error: () => {
        this.mensajeError.emit('Error al crear el recordatorio.');
        this.creandoRecordatorio = false;
      }
    });
  }

  eliminarRecordatorio(id: number) {
    if (!confirm('¿Eliminar este recordatorio?')) return;
    this.casoService.eliminarRecordatorio(id).subscribe({
      next: () => {
        this.mensajeExito.emit('Recordatorio eliminado correctamente.');
        this.recordatoriosActualizados.emit();
      },
      error: () => this.mensajeError.emit('Error al eliminar el recordatorio.')
    });
  }
}