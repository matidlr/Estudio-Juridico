import { Component, Input, Output, EventEmitter } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-caso-sidebar',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './caso-sidebar.component.html',
  styleUrl: './caso-sidebar.component.scss'
})
export class CasoSidebarComponent {
  @Input() caso: any = null;
  @Input() seccionActiva: string = 'info';
  @Input() archivosCount = 0;
  @Input() pruebasCount = 0;
  @Input() recordatoriosCount = 0;

  @Output() seccionCambiada = new EventEmitter<string>();

  cambiarSeccion(seccion: string) {
    this.seccionCambiada.emit(seccion);
  }
}