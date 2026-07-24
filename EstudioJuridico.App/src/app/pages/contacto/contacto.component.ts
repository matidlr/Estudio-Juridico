import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../shared/navbar/navbar.component';
import { FooterComponent } from '../../shared/footer/footer.component';
import { CasoService } from '../../services/caso.service';

@Component({
  selector: 'app-contacto',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule, NavbarComponent, FooterComponent],
  templateUrl: './contacto.component.html',
  styleUrl: './contacto.component.scss'
})
export class ContactoComponent {
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