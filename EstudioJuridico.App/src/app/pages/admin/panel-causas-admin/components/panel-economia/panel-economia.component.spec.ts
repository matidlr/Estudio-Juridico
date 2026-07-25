import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PanelEconomiaComponent } from './panel-economia.component';

describe('PanelEconomiaComponent', () => {
  let component: PanelEconomiaComponent;
  let fixture: ComponentFixture<PanelEconomiaComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PanelEconomiaComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PanelEconomiaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
