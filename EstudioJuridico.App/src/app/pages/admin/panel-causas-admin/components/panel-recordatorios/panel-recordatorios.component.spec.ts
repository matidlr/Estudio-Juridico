import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PanelRecordatoriosComponent } from './panel-recordatorios.component';

describe('PanelRecordatoriosComponent', () => {
  let component: PanelRecordatoriosComponent;
  let fixture: ComponentFixture<PanelRecordatoriosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PanelRecordatoriosComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PanelRecordatoriosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
