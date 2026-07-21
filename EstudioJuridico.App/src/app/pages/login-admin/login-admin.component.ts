import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../../shared/navbar/navbar.component';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login-admin',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, NavbarComponent],
  templateUrl: './login-admin.component.html',
  styleUrl: './login-admin.component.scss'
})
export class LoginAdminComponent implements OnInit {
  email = '';
  password = '';
  error = '';
  cargando = false;

  constructor(private authService: AuthService, private router: Router) {}

  ngOnInit() {
    // Si ya está logueado como admin lo redirigimos directo al panel
    if (this.authService.estaLogueado()) {
      const rol = this.authService.getRol();
      if (rol === 'Admin' || rol === 'SuperAdmin' || rol === 'Abogado') {
        this.router.navigate(['/admin/panel']);
      }
    }
  }

  login() {
    this.error = '';
    this.cargando = true;

    this.authService.loginAdmin(this.email, this.password).subscribe({
      next: (res) => {
        this.authService.guardarToken(res.data.token);
        const rol = this.authService.getRol();
        if (rol === 'Admin' || rol === 'SuperAdmin' || rol === 'Abogado') {
          this.router.navigate(['/admin/panel']);
        } else {
          this.authService.logout();
          this.error = 'No tenés permisos de administrador.';
          this.cargando = false;
        }
      },
      error: () => {
        this.error = 'Email o contraseña incorrectos.';
        this.cargando = false;
      }
    });
  }
}