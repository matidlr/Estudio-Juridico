import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class NotificacionService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getPreferencias() {
    return this.http.get<any>(`${this.apiUrl}/notificaciones/preferencias`);
  }

  actualizarPreferencias(preferencias: any) {
    return this.http.put(`${this.apiUrl}/notificaciones/preferencias`, preferencias);
  }
}