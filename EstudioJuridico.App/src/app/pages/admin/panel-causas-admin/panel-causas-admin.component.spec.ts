import { ComponentFixture, TestBed } from '@angular/core/testing';
import { PanelCausasAdminComponent } from './panel-causas-admin.component';

describe('PanelCausasAdminComponent', () => {
  let component: PanelCausasAdminComponent;
  let fixture: ComponentFixture<PanelCausasAdminComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [PanelCausasAdminComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(PanelCausasAdminComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});