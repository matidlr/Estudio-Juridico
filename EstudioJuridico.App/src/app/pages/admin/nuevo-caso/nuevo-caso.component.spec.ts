import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NuevoCasoComponent } from './nuevo-caso.component';

describe('NuevoCasoComponent', () => {
  let component: NuevoCasoComponent;
  let fixture: ComponentFixture<NuevoCasoComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NuevoCasoComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NuevoCasoComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
