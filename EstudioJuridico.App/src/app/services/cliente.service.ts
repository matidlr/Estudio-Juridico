import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ClienteService {
  private apiUrl = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getTodosClientes() {
    return this.http.get<any[]>(`${this.apiUrl}/clientes`);
  }

  getClientePorId(id: number) {
    return this.http.get<any>(`${this.apiUrl}/clientes/${id}`);
  }

  getMiPerfil() {
    return this.http.get<any>(`${this.apiUrl}/clientes/miperfil`);
  }

  actualizarMiPerfil(datos: any) {
    return this.http.put(`${this.apiUrl}/clientes/miperfil`, datos);
  }
}