import { ComponentFixture, TestBed } from '@angular/core/testing';
import { ErrorMessageComponent } from './error-message.component';

describe('ErrorMessageComponent', () => {
  let component: ErrorMessageComponent;
  let fixture: ComponentFixture<ErrorMessageComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ErrorMessageComponent]
    }).compileComponents();

    fixture = TestBed.createComponent(ErrorMessageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should not display when no message', () => {
    component.message = '';
    fixture.detectChanges();

    const element = fixture.nativeElement.querySelector('.error');
    expect(element).toBeFalsy();
  });

  it('should display error message', () => {
    component.message = 'Test error message';
    fixture.detectChanges();

    const element = fixture.nativeElement.querySelector('.error-text');
    expect(element.textContent).toContain('Test error message');
  });

  it('should show dismiss button when dismissible', () => {
    component.message = 'Test error';
    component.dismissible = true;
    fixture.detectChanges();

    const dismissButton = fixture.nativeElement.querySelector('.error-dismiss');
    expect(dismissButton).toBeTruthy();
  });

  it('should emit dismiss event when dismiss button clicked', () => {
    component.message = 'Test error';
    component.dismissible = true;
    fixture.detectChanges();

    spyOn(component.dismiss, 'emit');

    const dismissButton = fixture.nativeElement.querySelector('.error-dismiss');
    dismissButton.click();

    expect(component.dismiss.emit).toHaveBeenCalled();
  });
});
