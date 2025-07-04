import { ComponentFixture, TestBed } from '@angular/core/testing';
import { LoadingComponent } from './loading.component';

describe('LoadingComponent', () => {
  let component: LoadingComponent;
  let fixture: ComponentFixture<LoadingComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LoadingComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(LoadingComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display default message', () => {
    const element = fixture.nativeElement.querySelector('.loading');
    expect(element.textContent).toContain('Loading...');
  });

  it('should display custom message', () => {
    component.message = 'Custom loading message';
    fixture.detectChanges();

    const element = fixture.nativeElement.querySelector('.loading');
    expect(element.textContent).toContain('Custom loading message');
  });

  it('should apply size class', () => {
    component.size = 'large';
    fixture.detectChanges();

    const element = fixture.nativeElement.querySelector('.loading');
    expect(element.classList).toContain('loading-large');
  });
});
