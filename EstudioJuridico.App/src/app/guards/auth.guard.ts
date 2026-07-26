import { inject } from '@angular/core';
import { CanActivateFn, Router, ActivatedRouteSnapshot } from '@angular/router';

export const authGuard: CanActivateFn = (route: ActivatedRouteSnapshot) => {
  const router = inject(Router);
  const token = localStorage.getItem('token');

  if (token) {
    return true;
  }

  const url = route.pathFromRoot.map(r => r.url.map(s => s.path).join('/')).join('/');
  
  if (url.includes('admin')) {
    router.navigate(['/admin-estudio']);
  } else {
    router.navigate(['/login']);
  }

  return false;
};