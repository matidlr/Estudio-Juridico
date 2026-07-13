import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthService } from '../services/auth.service';

export const adminGuard: CanActivateFn = () => {
  const router = inject(Router);
  const authService = inject(AuthService);
  const token = localStorage.getItem('token');
  const rol = authService.getRol();

  if (token && (rol === 'Admin' || rol === 'Abogado' || rol === 'SuperAdmin')) {
    return true;
  }

  router.navigate(['/login-admin']);
  return false;
};