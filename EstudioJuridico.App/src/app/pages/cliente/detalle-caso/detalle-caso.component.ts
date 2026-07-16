import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ClienteSidebarComponent } from '../../../shared/cliente-sidebar/cliente-sidebar.component';
import { CasoService } from '../../../services/caso.service';

@Component({
  selector: 'app-detalle-caso',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, ClienteSidebarComponent],
  templateUrl: './detalle-caso.component.html',
  styleUrl: './detalle-caso.component.scss'
})
export class DetalleCasoComponent implements OnInit {
  caso: any = null;
  cargando = true;
  error = '';
  exito = '';
  seccionActiva = 'info';

  // Fojas
  busquedaFoja = '';
  fojasFiltradas: any[] = [];
  seccionSeleccionada: any = null;
  secciones: any[] = [];

  // Comentario
  comentario = '';
  enviandoComentario = false;

  respuesta = '';
  enviandoRespuesta = false;
  constructor(
    private route: ActivatedRoute,
    private casoService: CasoService
  ) {}

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.cargarCaso(id);
    this.cargarSecciones(id);
  }

  cargarCaso(id: number) {
    this.casoService.getCasoPorId(id).subscribe({
      next: (caso) => {
        this.caso = caso;
        this.fojasFiltradas = caso.actualizaciones ?? [];
        this.cargando = false;
      },
      error: () => {
        this.error = 'Error al cargar la causa.';
        this.cargando = false;
      }
    });
  }

  cargarSecciones(id: number) {
    this.casoService.getSeccionesDeCaso(id).subscribe({
      next: (secciones) => this.secciones = secciones,
      error: () => {}
    });
  }

  filtrarFojas() {
    let fojas = this.caso.actualizaciones ?? [];

    if (this.seccionSeleccionada) {
      fojas = fojas.filter((a: any) =>
        a.seccionExpedienteId === this.seccionSeleccionada.id
      );
    }

    if (this.busquedaFoja.trim()) {
      fojas = fojas.filter((a: any) =>
        a.nroFoja?.toLowerCase().includes(this.busquedaFoja.toLowerCase()) ||
        a.contenido?.toLowerCase().includes(this.busquedaFoja.toLowerCase())
      );
    }

    this.fojasFiltradas = fojas;
  }

  enviarComentario() {
    if (!this.comentario.trim()) return;
    this.enviandoComentario = true;

    this.casoService.agregarComentario({
      casoId:           this.caso.id,
      texto:            this.comentario,
      visibleAlAbogado: true
    }).subscribe({
      next: () => {
        this.exito = 'Consulta enviada al abogado correctamente.';
        this.comentario = '';
        this.enviandoComentario = false;
        setTimeout(() => this.exito = '', 3000);
      },
      error: () => {
        this.error = 'Error al enviar la consulta.';
        this.enviandoComentario = false;
      }
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
}