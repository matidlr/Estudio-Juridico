import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ClienteSidebarComponent } from './cliente-sidebar.component';

describe('ClienteSidebarComponent', () => {
  let component: ClienteSidebarComponent;
  let fixture: ComponentFixture<ClienteSidebarComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ClienteSidebarComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClienteSidebarComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
