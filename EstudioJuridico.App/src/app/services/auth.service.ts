import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient, private router: Router) {}

  login(email: string, password: string) {
    return this.http.post<{ token: string }>(`${this.apiUrl}/auth/login`, { email, password });
  }

  loginAdmin(email: string, password: string) {
    return this.http.post<{ token: string }>(`${this.apiUrl}/auth/login`, { email, password });
  }

  register(datos: any) {
    return this.http.post(`${this.apiUrl}/auth/register`, datos);
  }

  guardarToken(token: string) {
    localStorage.setItem('token', token);
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  getRol(): string | null {
    const token = this.getToken();
    if (!token) return null;

    // Decodificamos el JWT para obtener el rol
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload['http://schemas.microsoft.com/ws/2008/06/identity/claims/role'] ?? null;
  }

  getNombre(): string | null {
    const token = this.getToken();
    if (!token) return null;
    const payload = JSON.parse(atob(token.split('.')[1]));
    return payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress'] ?? null;
  }

  estaLogueado(): boolean {
    return !!this.getToken();
  }

  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['/']);
  }
}