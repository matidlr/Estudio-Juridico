import { Component } from '@angular/core';
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
export class LoginAdminComponent {
  email = '';
  password = '';
  error = '';
  cargando = false;

  constructor(private authService: AuthService, private router: Router) {}

  login() {
    this.error = '';
    this.cargando = true;

    this.authService.loginAdmin(this.email, this.password).subscribe({
      next: (res) => {
        this.authService.guardarToken(res.token);

        if (this.authService.getRol() === 'Admin') {
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