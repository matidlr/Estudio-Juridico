import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CasoService } from '../../../../../services/caso.service';
import { AuthService } from '../../../../../services/auth.service';

@Component({
  selector: 'app-panel-consultas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './panel-consultas.component.html',
  styleUrl: './panel-consultas.component.scss'
})
export class PanelConsultasComponent {
  @Input() caso: any;
  @Output() casoActualizado = new EventEmitter<void>();
  @Output() mensajeExito = new EventEmitter<string>();
  @Output() mensajeError = new EventEmitter<string>();

  respuesta = '';
  enviandoRespuesta = false;

  constructor(private casoService: CasoService, public authService: AuthService) {}

  responderConsulta() {
    if (!this.respuesta.trim()) return;
    this.enviandoRespuesta = true;

    this.casoService.responderComentario(this.caso.id, this.respuesta).subscribe({
      next: () => {
        this.mensajeExito.emit('Respuesta enviada correctamente.');
        this.respuesta = '';
        this.enviandoRespuesta = false;
        this.casoActualizado.emit();
      },
      error: () => {
        this.mensajeError.emit('Error al enviar la respuesta.');
        this.enviandoRespuesta = false;
      }
    });
  }
}