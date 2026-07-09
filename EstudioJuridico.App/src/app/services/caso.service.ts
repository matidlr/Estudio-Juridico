import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CasoService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getMisCasos() {
    return this.http.get<any[]>(`${this.apiUrl}/casos/mios`);
  }

  getCasoPorId(id: number) {
    return this.http.get<any>(`${this.apiUrl}/casos/${id}`);
  }

  getTodosCasos() {
    return this.http.get<any[]>(`${this.apiUrl}/casos`);
  }

  crearCaso(caso: any) {
    return this.http.post(`${this.apiUrl}/casos`, caso);
  }

  editarCaso(id: number, caso: any) {
    return this.http.put(`${this.apiUrl}/casos/${id}`, caso);
  }

  agregarActualizacion(actualizacion: any) {
    return this.http.post(`${this.apiUrl}/casos/actualizacion`, actualizacion);
  }

  agregarComentario(comentario: any) {
    return this.http.post(`${this.apiUrl}/casos/comentario`, comentario);
  }
}