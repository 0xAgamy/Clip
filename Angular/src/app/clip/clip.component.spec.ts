import { provideHttpClient } from '@angular/common/http';
import { provideHttpClientTesting } from '@angular/common/http/testing';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { provideRouter } from '@angular/router';

import { ClipComponent } from './clip.component';

describe('ClipComponent', () => {
  let component: ClipComponent;
  let fixture: ComponentFixture<ClipComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ClipComponent ],
      providers: [ provideHttpClient(), provideHttpClientTesting(), provideRouter([]) ],
    })
    .compileComponents();

    fixture = TestBed.createComponent(ClipComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
