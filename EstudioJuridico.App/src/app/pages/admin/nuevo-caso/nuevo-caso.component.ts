import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AdminSidebarComponent } from '../../../shared/admin-sidebar/admin-sidebar.component';
import { CasoService } from '../../../services/caso.service';
import { ClienteService } from '../../../services/cliente.service';

@Component({
  selector: 'app-nuevo-caso',
  standalone: true,
  imports: [CommonModule, FormsModule, AdminSidebarComponent],
  templateUrl: './nuevo-caso.component.html',
  styleUrl: './nuevo-caso.component.scss'
})
export class NuevoCasoComponent implements OnInit {
  clientes: any[] = [];
  caso = {
  caratula: '',
  nroExpediente: '',
  juzgado: '',
  proceso: 'Ordinario',
  tipo: 'Laboral',
  estado: 'Activo',
  etapa: 'Consulta inicial',
  clienteId: 0
};
  error = '';
  exito = '';
  cargando = false;

  tipos = ['Laboral', 'Civil', 'Penal', 'Familia', 'Comercial'];
  procesos = [
    'Ordinario', 'Sumarísimo', 'Ejecutivo',
    'Amparo', 'Cautelar', 'Incidente', 'Recurso'
  ];
  etapas = [
    'Consulta inicial', 'Mediación', 'Demanda presentada',
    'Instrucción / prueba', 'Audiencia', 'Alegatos',
    'Sentencia', 'Apelación', 'Resolución final'
  ];

  constructor(
    private casoService: CasoService,
    private clienteService: ClienteService,
    private router: Router
  ) {}

  ngOnInit() {
    this.clienteService.getTodosClientes().subscribe({
      next: (clientes) => {
        this.clientes = clientes;
        if (clientes.length > 0) {
          this.caso.clienteId = clientes[0].id;
        }
      },
      error: () => {
        this.error = 'Error al cargar los clientes.';
      }
    });
  }

  crear() {
    if (!this.caso.caratula || !this.caso.clienteId) {
      this.error = 'La carátula y el cliente son obligatorios.';
      return;
    }

    this.cargando = true;
    this.error = '';

    
    this.casoService.crearCaso({
      caratula:      this.caso.caratula,
      nroExpediente: this.caso.nroExpediente,
      juzgado:       this.caso.juzgado,
      proceso:       this.caso.proceso,
      tipo:          this.caso.tipo,
      estado:        this.caso.estado,
      etapa:         this.caso.etapa,
      clienteId:     this.caso.clienteId
    }).subscribe({
      next: () => {
        this.exito = 'Causa creada correctamente.';
        this.cargando = false;
        setTimeout(() => this.router.navigate(['/admin/panel']), 2000);
      },
      error: () => {
        this.error = 'Error al crear la causa.';
        this.cargando = false;
      }
    });
  }
}