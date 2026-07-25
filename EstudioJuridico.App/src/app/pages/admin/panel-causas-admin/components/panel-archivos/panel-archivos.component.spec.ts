import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PanelArchivosComponent } from './panel-archivos.component';

describe('PanelArchivosComponent', () => {
  let component: PanelArchivosComponent;
  let fixture: ComponentFixture<PanelArchivosComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PanelArchivosComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PanelArchivosComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
