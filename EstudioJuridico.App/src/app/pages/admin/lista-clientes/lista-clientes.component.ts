import { Component, OnInit } from '@angular/core';
import { RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { NavbarComponent } from '../../../shared/navbar/navbar.component';
import { ClienteService } from '../../../services/cliente.service';
import { AdminSidebarComponent } from '../../../shared/admin-sidebar/admin-sidebar.component';

@Component({
  selector: 'app-lista-clientes',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink, NavbarComponent, AdminSidebarComponent],
  templateUrl: './lista-clientes.component.html',
  styleUrl: './lista-clientes.component.scss'
})
export class ListaClientesComponent implements OnInit {
  clientes: any[] = [];
  clientesFiltrados: any[] = [];
  busqueda = '';
  cargando = true;
  error = '';

  constructor(private clienteService: ClienteService) {}

  ngOnInit() {
    this.clienteService.getTodosClientes().subscribe({
      next: (clientes) => {
        this.clientes = clientes;
        this.clientesFiltrados = clientes;
        this.cargando = false;
      },
      error: () => {
        this.error = 'Error al cargar los clientes.';
        this.cargando = false;
      }
    });
  }

  filtrar() {
    this.clientesFiltrados = this.clientes.filter(c =>
      this.busqueda === '' ||
      c.nombre.toLowerCase().includes(this.busqueda.toLowerCase()) ||
      c.apellido.toLowerCase().includes(this.busqueda.toLowerCase()) ||
      c.email.toLowerCase().includes(this.busqueda.toLowerCase())
    );
  }
}