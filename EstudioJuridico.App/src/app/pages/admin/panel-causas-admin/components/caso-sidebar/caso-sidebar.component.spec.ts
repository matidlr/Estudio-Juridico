import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CasoSidebarComponent } from './caso-sidebar.component';

describe('CasoSidebarComponent', () => {
  let component: CasoSidebarComponent;
  let fixture: ComponentFixture<CasoSidebarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CasoSidebarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CasoSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
