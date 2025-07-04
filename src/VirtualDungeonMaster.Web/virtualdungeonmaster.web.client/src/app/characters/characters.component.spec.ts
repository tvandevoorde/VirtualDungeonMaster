import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { of } from 'rxjs';

import { CharactersComponent } from './characters.component';
import { CharacterService } from '../services/character.service';
import { CharacterCardComponent } from '../components/character-card/character-card.component';
import { CharacterFormComponent } from '../components/character-form/character-form.component';
import { LoadingComponent } from '../components/loading/loading.component';
import { ErrorMessageComponent } from '../components/error-message/error-message.component';
import { Character } from '../models/character.model';
import { FormsModule } from '@angular/forms';

describe('CharactersComponent', () => {
  let component: CharactersComponent;
  let fixture: ComponentFixture<CharactersComponent>;
  let mockCharacterService: jasmine.SpyObj<CharacterService>;

  const mockCharacters: Character[] = [
    {
      id: 1,
      name: 'Test Hero',
      class: 'fighter',
      race: 'human',
      background: 'Test background'
    }
  ];

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('CharacterService', [
      'getCharacters',
      'createCharacter',
      'updateCharacter',
      'deleteCharacter'
    ]);

    await TestBed.configureTestingModule({
      declarations: [
        CharactersComponent,
        CharacterCardComponent,
        CharacterFormComponent,
        LoadingComponent,
        ErrorMessageComponent
      ],
      imports: [RouterTestingModule, FormsModule],
      providers: [
        { provide: CharacterService, useValue: spy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CharactersComponent);
    component = fixture.componentInstance;
    mockCharacterService = TestBed.inject(CharacterService) as jasmine.SpyObj<CharacterService>;

    // Setup default mock returns
    mockCharacterService.getCharacters.and.returnValue(of(mockCharacters));
    mockCharacterService.createCharacter.and.returnValue(of(mockCharacters[0]));
    mockCharacterService.updateCharacter.and.returnValue(of(mockCharacters[0]));
    mockCharacterService.deleteCharacter.and.returnValue(of(void 0));

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load characters on init', () => {
    expect(mockCharacterService.getCharacters).toHaveBeenCalled();
    expect(component.characters.length).toBe(1);
    expect(component.characters[0].name).toBe('Test Hero');
  });

  it('should show create form', () => {
    component.showCreateForm();
    expect(component.showForm).toBe(true);
    expect(component.editingCharacter).toBeNull();
  });

  it('should show edit form', () => {
    const character = mockCharacters[0];
    component.editCharacter(character);
    expect(component.showForm).toBe(true);
    expect(component.editingCharacter).toBe(character);
  });

  it('should cancel form', () => {
    component.showForm = true;
    component.editingCharacter = mockCharacters[0];
    component.error = 'Test error';

    component.onCancelForm();

    expect(component.showForm).toBe(false);
    expect(component.editingCharacter).toBeNull();
    expect(component.error).toBe('');
  });

  it('should clear error', () => {
    component.error = 'Test error';
    component.clearError();
    expect(component.error).toBe('');
  });

  it('should create character', () => {
    const newCharacter: Character = {
      name: 'New Hero',
      class: 'wizard',
      race: 'elf',
      background: 'New background'
    };

    component.onSaveCharacter(newCharacter);

    expect(mockCharacterService.createCharacter).toHaveBeenCalledWith(newCharacter);
  });

  it('should update character', () => {
    const existingCharacter = { ...mockCharacters[0] };
    component.editingCharacter = existingCharacter;

    const updatedCharacter: Character = {
      ...existingCharacter,
      name: 'Updated Hero'
    };

    component.onSaveCharacter(updatedCharacter);

    expect(mockCharacterService.updateCharacter).toHaveBeenCalledWith(
      existingCharacter.id!,
      { ...updatedCharacter, id: existingCharacter.id }
    );
  });
});
