import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PanelConsultasComponent } from './panel-consultas.component';

describe('PanelConsultasComponent', () => {
  let component: PanelConsultasComponent;
  let fixture: ComponentFixture<PanelConsultasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PanelConsultasComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PanelConsultasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
