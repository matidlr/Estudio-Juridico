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

  eliminarCaso(id: number) {
  return this.http.delete(`${this.apiUrl}/casos/${id}`);
}

  // Subir archivo adjunto al caso
subirArchivo(casoId: number, categoria: string, archivo: File) {
  const formData = new FormData();
  formData.append('casoId', casoId.toString());
  formData.append('categoria', categoria);
  formData.append('archivo', archivo);
  return this.http.post<any>(`${this.apiUrl}/archivos/subir`, formData);
}

// Obtener archivos de un caso
getArchivosDeCaso(casoId: number) {
  return this.http.get<any[]>(`${this.apiUrl}/archivos/caso/${casoId}`);
}

// Eliminar archivo
eliminarArchivo(id: number) {
  return this.http.delete(`${this.apiUrl}/archivos/${id}`);
}

// Subir prueba
subirPrueba(casoId: number, descripcion: string, archivo: File) {
  const formData = new FormData();
  formData.append('casoId', casoId.toString());
  formData.append('descripcion', descripcion);
  formData.append('archivo', archivo);
  return this.http.post<any>(`${this.apiUrl}/pruebas/subir`, formData);
}

// Obtener pruebas de un caso
getPruebasDeCaso(casoId: number) {
  return this.http.get<any[]>(`${this.apiUrl}/pruebas/caso/${casoId}`);
}

// Eliminar prueba
eliminarPrueba(id: number) {
  return this.http.delete(`${this.apiUrl}/pruebas/${id}`);
}

// Recordatorios
getRecordatoriosDeCaso(casoId: number) {
  return this.http.get<any[]>(`${this.apiUrl}/recordatorios/caso/${casoId}`);
}

crearRecordatorio(recordatorio: any) {
  return this.http.post(`${this.apiUrl}/recordatorios`, recordatorio);
}

eliminarRecordatorio(id: number) {
  return this.http.delete(`${this.apiUrl}/recordatorios/${id}`);
}

reasignarAbogado(casoId: number, abogadoId: number) {
  return this.http.put(`${this.apiUrl}/casos/${casoId}/reasignar`, { abogadoId });
}

getAbogados() {
  return this.http.get<any[]>(`${this.apiUrl}/abogados`);
}
getTodosRecordatorios() {
  return this.http.get<any[]>(`${this.apiUrl}/recordatorios`);
}
getProximos() {
  return this.http.get<any[]>(`${this.apiUrl}/recordatorios/proximos`);
}

getUltimasActualizaciones() {
  return this.http.get<any[]>(`${this.apiUrl}/actualizaciones/ultimas`);
}

getSeccionesDeCaso(casoId: number) {
  return this.http.get<any[]>(`${this.apiUrl}/secciones/caso/${casoId}`);
}

crearSeccion(seccion: any) {
  return this.http.post(`${this.apiUrl}/secciones`, seccion);
}

eliminarSeccion(id: number) {
  return this.http.delete(`${this.apiUrl}/secciones/${id}`);
}
responderComentario(casoId: number, texto: string) {
  return this.http.post(`${this.apiUrl}/casos/comentario`, {
    casoId,
    texto,
    visibleAlAbogado: true,
    tipoAutor: 'Abogado'
  });
}

getConsultasPendientes() {
  return this.http.get<any[]>(`${this.apiUrl}/casos/admin/consultas-pendientes`);
}

eliminarComentario(id: number) {
  return this.http.delete(`${this.apiUrl}/casos/comentario/${id}`);
}

marcarComentarioLeido(id: number) {
  return this.http.put(`${this.apiUrl}/casos/comentario/${id}/leida`, {});
}
}