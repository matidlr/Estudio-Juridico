import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ConsultasNuevosClientesComponent } from './consultas-nuevos-clientes.component';

describe('ConsultasNuevosClientesComponent', () => {
  let component: ConsultasNuevosClientesComponent;
  let fixture: ComponentFixture<ConsultasNuevosClientesComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ConsultasNuevosClientesComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ConsultasNuevosClientesComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
