import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CasoService } from '../../../../../services/caso.service';
import { AuthService } from '../../../../../services/auth.service';

@Component({
  selector: 'app-panel-info',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './panel-info.component.html',
  styleUrl: './panel-info.component.scss'
})
export class PanelInfoComponent implements OnInit {
  @Input() caso: any;
  @Input() abogados: any[] = [];
  @Output() casoActualizado = new EventEmitter<void>();
  @Output() mensajeExito = new EventEmitter<string>();
  @Output() mensajeError = new EventEmitter<string>();

  editando = false;
  reasignando = false;
  mostrarReasignar = false;
  abogadoSeleccionado = 0;
  permisos: any[] = [];
  abogadoPermisoId = 0;

  tipos = ['Laboral', 'Civil', 'Penal', 'Familia', 'Comercial'];
  etapas = [
    'Consulta inicial', 'Mediación', 'Demanda presentada',
    'Instrucción / prueba', 'Audiencia', 'Alegatos',
    'Sentencia', 'Apelación', 'Resolución final'
  ];
  estados = ['Activo', 'Suspendido', 'Finalizado', 'Archivado'];

  constructor(
    private casoService: CasoService,
    public authService: AuthService
  ) {}

  ngOnInit() {
    this.cargarPermisos();
  }

  cargarPermisos() {
    this.casoService.getPermisosCausa(this.caso.id).subscribe({
      next: (data) => this.permisos = data.datos ?? data,
      error: () => {}
    });
  }

  guardarCambios() {
    this.casoService.editarCaso(this.caso.id, this.caso).subscribe({
      next: () => {
        this.editando = false;
        this.mensajeExito.emit('Caso actualizado correctamente.');
        this.casoActualizado.emit();
      },
      error: () => this.mensajeError.emit('Error al actualizar el caso.')
    });
  }

  reasignarAbogado() {
    if (!this.abogadoSeleccionado) return;
    this.reasignando = true;

    this.casoService.reasignarAbogado(this.caso.id, this.abogadoSeleccionado).subscribe({
      next: () => {
        this.mensajeExito.emit('Abogado reasignado correctamente.');
        this.reasignando = false;
        this.mostrarReasignar = false;
        this.abogadoSeleccionado = 0;
        this.casoActualizado.emit();
      },
      error: () => {
        this.mensajeError.emit('Error al reasignar el abogado.');
        this.reasignando = false;
      }
    });
  }

  darPermiso() {
    if (!this.abogadoPermisoId) return;
    this.casoService.darPermiso(this.caso.id, this.abogadoPermisoId).subscribe({
      next: () => {
        this.mensajeExito.emit('Permiso otorgado correctamente.');
        this.abogadoPermisoId = 0;
        this.cargarPermisos();
      },
      error: (err) => this.mensajeError.emit(err.error?.mensaje ?? 'Error al otorgar permiso.')
    });
  }

  revocarPermiso(id: number) {
    if (!confirm('¿Revocar este permiso?')) return;
    this.casoService.revocarPermiso(id).subscribe({
      next: () => {
        this.mensajeExito.emit('Permiso revocado correctamente.');
        this.cargarPermisos();
      },
      error: () => this.mensajeError.emit('Error al revocar el permiso.')
    });
  }
}