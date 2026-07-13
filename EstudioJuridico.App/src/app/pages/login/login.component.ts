import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { NavbarComponent } from '../../shared/navbar/navbar.component';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, NavbarComponent],
  templateUrl: './login.component.html',
  styleUrl: './login.component.scss'
})
export class LoginComponent {
  email = '';
  password = '';
  error = '';
  cargando = false;

  constructor(private authService: AuthService, private router: Router) {}
  ngOnInit() {
  if (this.authService.estaLogueado()) {
    const rol = this.authService.getRol();
    if (rol === 'Cliente') {
      this.router.navigate(['/cliente/panel']);
    }
  }
}

  login() {
    this.error = '';
    this.cargando = true;

    this.authService.login(this.email, this.password).subscribe({
      next: (res) => {
        this.authService.guardarToken(res.token);
        this.router.navigate(['/cliente/panel']);
      },
      error: () => {
        this.error = 'Email o contraseña incorrectos.';
        this.cargando = false;
      }
    });
  }
}