import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule } from '@angular/forms';
import { CharacterFormComponent } from './character-form.component';
import { Character } from '../../models/character.model';

describe('CharacterFormComponent', () => {
  let component: CharacterFormComponent;
  let fixture: ComponentFixture<CharacterFormComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [CharacterFormComponent],
      imports: [FormsModule]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CharacterFormComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should initialize with empty form', () => {
    expect(component.form.name).toBe('');
    expect(component.form.class).toBe('');
    expect(component.form.race).toBe('');
    expect(component.form.background).toBe('');
  });

  it('should populate form when character is provided', () => {
    const testCharacter: Character = {
      id: 1,
      name: 'Test Hero',
      class: 'fighter',
      race: 'human',
      background: 'A brave warrior'
    };

    component.character = testCharacter;
    component.ngOnInit();

    expect(component.form.name).toBe('Test Hero');
    expect(component.form.class).toBe('fighter');
    expect(component.form.race).toBe('human');
    expect(component.form.background).toBe('A brave warrior');
  });

  it('should validate form correctly', () => {
    expect(component.isFormValid()).toBeFalsy();

    component.form.name = 'Test';
    expect(component.isFormValid()).toBeFalsy();

    component.form.class = 'fighter';
    expect(component.isFormValid()).toBeFalsy();

    component.form.race = 'human';
    expect(component.isFormValid()).toBeTruthy();
  });

  it('should emit save event on valid form submission', () => {
    spyOn(component.save, 'emit');

    component.form = {
      name: 'Test Hero',
      class: 'fighter',
      race: 'human',
      background: 'Test background'
    };

    component.onSubmit();

    expect(component.save.emit).toHaveBeenCalledWith(component.form);
  });

  it('should not emit save event on invalid form submission', () => {
    spyOn(component.save, 'emit');

    component.form.name = ''; // Invalid form

    component.onSubmit();

    expect(component.save.emit).not.toHaveBeenCalled();
  });

  it('should emit cancel event', () => {
    spyOn(component.cancel, 'emit');

    component.onCancel();

    expect(component.cancel.emit).toHaveBeenCalled();
  });

  it('should find character class by id', () => {
    const fighterClass = component.getCharacterClass('fighter');
    expect(fighterClass?.name).toBe('Fighter');
    expect(fighterClass?.icon).toBe('🛡️');
  });

  it('should find race by id', () => {
    const humanRace = component.getRace('human');
    expect(humanRace?.name).toBe('Human');
    expect(humanRace?.icon).toBe('👤');
  });

  it('should generate dynamic background placeholder', () => {
    component.form.name = 'Aragorn';
    component.form.class = 'ranger';
    component.form.race = 'human';

    const placeholder = component.getBackgroundPlaceholder();
    expect(placeholder).toContain('Aragorn');
    expect(placeholder).toContain('Human');
    expect(placeholder).toContain('Ranger');
  });

  it('should reset form', () => {
    component.form = {
      name: 'Test',
      class: 'fighter',
      race: 'human',
      background: 'Test'
    };

    component.resetForm();

    expect(component.form.name).toBe('');
    expect(component.form.class).toBe('');
    expect(component.form.race).toBe('');
    expect(component.form.background).toBe('');
  });
});
