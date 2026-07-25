import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CasoService } from '../../../../../services/caso.service';
import { AuthService } from '../../../../../services/auth.service';

@Component({
  selector: 'app-panel-economia',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './panel-economia.component.html',
  styleUrl: './panel-economia.component.scss'
})
export class PanelEconomiaComponent implements OnInit {
  @Input() caso: any;
  @Input() movimientos: any[] = [];
  @Input() resumenEconomico: any = null;
  @Output() movimientosActualizados = new EventEmitter<void>();
  @Output() mensajeExito = new EventEmitter<string>();
  @Output() mensajeError = new EventEmitter<string>();

  nuevoMovimiento = {
    tipo: 'Honorario',
    concepto: '',
    monto: 0,
    fecha: new Date().toISOString().split('T')[0],
    formaPago: '',
    notas: ''
  };
  guardandoMovimiento = false;

  constructor(
    private casoService: CasoService,
    public authService: AuthService
  ) {}

  ngOnInit() {}

  getColorMovimiento(tipo: string): string {
    if (tipo === 'Pago') return 'movimiento-pago';
    if (tipo === 'Gasto') return 'movimiento-gasto';
    return 'movimiento-honorario';
  }

  crearMovimiento() {
    if (!this.nuevoMovimiento.concepto || !this.nuevoMovimiento.monto) return;
    this.guardandoMovimiento = true;

    this.casoService.crearMovimiento({
      ...this.nuevoMovimiento,
      casoId: this.caso.id
    }).subscribe({
      next: () => {
        this.mensajeExito.emit('Movimiento registrado correctamente.');
        this.guardandoMovimiento = false;
        this.nuevoMovimiento = {
          tipo: 'Honorario', concepto: '', monto: 0,
          fecha: new Date().toISOString().split('T')[0],
          formaPago: '', notas: ''
        };
        this.movimientosActualizados.emit();
      },
      error: () => {
        this.mensajeError.emit('Error al registrar el movimiento.');
        this.guardandoMovimiento = false;
      }
    });
  }

  eliminarMovimiento(id: number) {
    if (!confirm('¿Eliminar este movimiento?')) return;
    this.casoService.eliminarMovimiento(id).subscribe({
      next: () => {
        this.mensajeExito.emit('Movimiento eliminado correctamente.');
        this.movimientosActualizados.emit();
      },
      error: () => this.mensajeError.emit('Error al eliminar el movimiento.')
    });
  }
}