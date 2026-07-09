import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../../shared/navbar/navbar.component';
import { CasoService } from '../../../services/caso.service';

@Component({
  selector: 'app-detalle-caso',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, NavbarComponent],
  templateUrl: './detalle-caso.component.html',
  styleUrl: './detalle-caso.component.scss'
})
export class DetalleCasoComponent implements OnInit {
  caso: any = null;
  comentario = '';
  cargando = true;
  enviando = false;
  error = '';
  exito = '';

  constructor(
    private route: ActivatedRoute,
    private casoService: CasoService
  ) {}

  ngOnInit() {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.casoService.getCasoPorId(id).subscribe({
      next: (caso) => {
        this.caso = caso;
        this.cargando = false;
      },
      error: () => {
        this.error = 'Error al cargar el caso.';
        this.cargando = false;
      }
    });
  }

  enviarComentario() {
    if (!this.comentario.trim()) return;
    this.enviando = true;

    this.casoService.agregarComentario({
      casoId: this.caso.id,
      texto: this.comentario,
      visibleAlAbogado: true
    }).subscribe({
      next: () => {
        this.exito = 'Comentario enviado correctamente.';
        this.comentario = '';
        this.enviando = false;
        setTimeout(() => this.exito = '', 3000);
      },
      error: () => {
        this.error = 'Error al enviar el comentario.';
        this.enviando = false;
      }
    });
  }

  getBadgeEstado(estado: string): string {
    const badges: any = {
      'Activo': 'badge-activo',
      'Suspendido': 'badge-suspendido',
      'Finalizado': 'badge-cerrado',
      'Archivado': 'badge-cerrado'
    };
    return badges[estado] ?? 'badge-cerrado';
  }

  getBadgeTipo(tipo: string): string {
    const badges: any = {
      'Laboral': 'badge-laboral',
      'Civil': 'badge-civil',
      'Penal': 'badge-penal',
      'Familia': 'badge-familia',
      'Comercial': 'badge-comercial'
    };
    return badges[tipo] ?? '';
  }
}