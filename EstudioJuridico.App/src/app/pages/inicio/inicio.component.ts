import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../shared/navbar/navbar.component';
import { CasoService } from '../../services/caso.service';

@Component({
  selector: 'app-inicio',
  standalone: true,
  imports: [CommonModule, RouterLink, NavbarComponent, FormsModule],
  templateUrl: './inicio.component.html',
  styleUrl: './inicio.component.scss'
})
export class InicioComponent {
  consulta = {
    nombre: '',
    email: '',
    telefono: '',
    mensaje: '',
    areaInteres: ''
  };
  enviandoConsulta = false;
  consultaEnviada = false;

  constructor(private casoService: CasoService) {}

  enviarConsulta() {
    if (!this.consulta.nombre || !this.consulta.email || !this.consulta.mensaje) return;
    this.enviandoConsulta = true;

    this.casoService.enviarConsultaPublica(this.consulta).subscribe({
      next: () => {
        this.consultaEnviada = true;
        this.enviandoConsulta = false;
      },
      error: () => {
        this.enviandoConsulta = false;
      }
    });
  }
}