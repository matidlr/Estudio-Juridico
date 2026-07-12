import { Component, OnInit } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../../shared/navbar/navbar.component';
import { CasoService } from '../../../services/caso.service';
import { ClienteService } from '../../../services/cliente.service';
import { AdminSidebarComponent } from '../../../shared/admin-sidebar/admin-sidebar.component';

@Component({
  selector: 'app-nuevo-caso',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, NavbarComponent, AdminSidebarComponent],
  templateUrl: './nuevo-caso.component.html',
  styleUrl: './nuevo-caso.component.scss'
})
export class NuevoCasoComponent implements OnInit {
  clientes: any[] = [];
  caso = {
    titulo: '',
    nombrePartes: '',
    descripcion: '',
    tipo: 'Laboral',
    estado: 'Activo',
    etapa: 'Consulta inicial',
    clienteId: 0
  };
  error = '';
  exito = '';
  cargando = false;

  tipos = ['Laboral', 'Civil', 'Penal', 'Familia', 'Comercial'];
  etapas = [
    'Consulta inicial',
    'Mediación',
    'Demanda presentada',
    'Instrucción / prueba',
    'Audiencia',
    'Alegatos',
    'Sentencia',
    'Apelación',
    'Resolución final'
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
    if (!this.caso.titulo || !this.caso.clienteId) {
      this.error = 'El título y el cliente son obligatorios.';
      return;
    }

    this.cargando = true;
    this.error = '';

    this.casoService.crearCaso(this.caso).subscribe({
      next: () => {
        this.exito = 'Caso creado correctamente.';
        this.cargando = false;
        setTimeout(() => this.router.navigate(['/admin/panel']), 2000);
      },
      error: () => {
        this.error = 'Error al crear el caso.';
        this.cargando = false;
      }
    });
  }
}