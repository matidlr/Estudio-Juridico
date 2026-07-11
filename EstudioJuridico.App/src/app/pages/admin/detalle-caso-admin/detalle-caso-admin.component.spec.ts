import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DetalleCasoAdminComponent } from './detalle-caso-admin.component';

describe('DetalleCasoAdminComponent', () => {
  let component: DetalleCasoAdminComponent;
  let fixture: ComponentFixture<DetalleCasoAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DetalleCasoAdminComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DetalleCasoAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
