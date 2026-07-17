import { Routes } from '@angular/router';
import { InicioComponent } from './pages/inicio/inicio.component';
import { LoginComponent } from './pages/login/login.component';
import { RegistroComponent } from './pages/registro/registro.component';
import { LoginAdminComponent } from './pages/login-admin/login-admin.component';
import { PanelClienteComponent } from './pages/cliente/panel-cliente/panel-cliente.component';
import { DetalleCasoComponent } from './pages/cliente/detalle-caso/detalle-caso.component';
import { MiPerfilComponent } from './pages/cliente/mi-perfil/mi-perfil.component';
import { PanelAdminComponent } from './pages/admin/panel-admin/panel-admin.component';
import { NuevoCasoComponent } from './pages/admin/nuevo-caso/nuevo-caso.component';
import { ListaClientesComponent } from './pages/admin/lista-clientes/lista-clientes.component';
import { authGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';
import { DetalleCasoAdminComponent } from './pages/admin/detalle-caso-admin/detalle-caso-admin.component';
import { CalendarioComponent } from './pages/admin/calendario/calendario.component';
import { NotificacionesComponent } from './pages/admin/notificaciones/notificaciones.component';
import { MiCuentaComponent } from './pages/cliente/mi-cuenta/mi-cuenta.component';

export const routes: Routes = [
  { path: '', component: InicioComponent },
  { path: 'login', component: LoginComponent },
  { path: 'registro', component: RegistroComponent },
  { path: 'login-admin', component: LoginAdminComponent },
  {
    path: 'cliente',
    canActivate: [authGuard],
    children: [
      { path: 'panel', component: PanelClienteComponent },
      { path: 'caso/:id', component: DetalleCasoComponent },
      { path: 'perfil', component: MiPerfilComponent },
      { path: 'cuenta', component: MiCuentaComponent },
      { path: '', redirectTo: 'panel', pathMatch: 'full' }
    ]
  },
 {
  path: 'admin',
  canActivate: [adminGuard],
  children: [
    { path: 'panel', component: PanelAdminComponent },
    { path: 'nuevo-caso', component: NuevoCasoComponent },
    { path: 'clientes', component: ListaClientesComponent },
    { path: 'caso/:id', component: DetalleCasoAdminComponent },
    { path: 'calendario', component: CalendarioComponent },
    { path: 'notificaciones', component: NotificacionesComponent },
    { path: '', redirectTo: 'panel', pathMatch: 'full' }
  ]
},
];