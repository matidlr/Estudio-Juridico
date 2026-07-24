import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../../shared/navbar/navbar.component';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-registro',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, NavbarComponent],
  templateUrl: './registro.component.html',
  styleUrl: './registro.component.scss'
})
export class RegistroComponent {
  nombre = '';
  apellido = '';
  email = '';
  password = '';
  telefono = '';
  error = '';
  exito = '';
  cargando = false;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
  if (this.authService.estaLogueado()) {
    const rol = this.authService.getRol();
    if (rol === 'Cliente') {
      this.router.navigate(['/cliente/panel']);
    } else {
      this.router.navigate(['/admin/panel']);
    }
  }
}
  registrar() {
    this.error = '';
    this.exito = '';
    this.cargando = true;

    const datos = {
      nombre: this.nombre,
      apellido: this.apellido,
      email: this.email,
      password: this.password,
      telefono: this.telefono
    };

    this.authService.register(datos).subscribe({
      next: () => {
        this.exito = 'Cuenta creada correctamente. Podés ingresar ahora.';
        this.cargando = false;
        setTimeout(() => this.router.navigate(['/login']), 2000);
      },
      error: (err) => {
        this.error = err.error || 'Error al crear la cuenta.';
        this.cargando = false;
      }
    });
  }
}