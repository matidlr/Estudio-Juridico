import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PanelPruebasComponent } from './panel-pruebas.component';

describe('PanelPruebasComponent', () => {
  let component: PanelPruebasComponent;
  let fixture: ComponentFixture<PanelPruebasComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PanelPruebasComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PanelPruebasComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
