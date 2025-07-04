import { ComponentFixture, TestBed } from '@angular/core/testing';
import { CharacterCardComponent } from './character-card.component';
import { CharacterDisplayService } from '../../services/character-display.service';
import { Character } from '../../models/character.model';

describe('CharacterCardComponent', () => {
  let component: CharacterCardComponent;
  let fixture: ComponentFixture<CharacterCardComponent>;
  let mockCharacterDisplayService: jasmine.SpyObj<CharacterDisplayService>;

  const mockCharacter: Character = {
    id: 1,
    name: 'Test Character',
    class: 'fighter',
    race: 'human',
    background: 'Test background'
  };

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('CharacterDisplayService', ['getClassInfo', 'getRaceInfo']);

    await TestBed.configureTestingModule({
      declarations: [CharacterCardComponent],
      providers: [
        { provide: CharacterDisplayService, useValue: spy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(CharacterCardComponent);
    component = fixture.componentInstance;
    mockCharacterDisplayService = TestBed.inject(CharacterDisplayService) as jasmine.SpyObj<CharacterDisplayService>;

    // Set up mock returns
    mockCharacterDisplayService.getClassInfo.and.returnValue({ name: 'Fighter', icon: '🛡️' });
    mockCharacterDisplayService.getRaceInfo.and.returnValue({ name: 'Human', icon: '👤' });

    component.character = mockCharacter;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should display character name', () => {
    const nameElement = fixture.nativeElement.querySelector('.character-name');
    expect(nameElement.textContent).toContain('Test Character');
  });

  it('should emit characterSelect when clicked and selectable', () => {
    component.selectable = true;
    spyOn(component.characterSelect, 'emit');

    const cardElement = fixture.nativeElement.querySelector('.character-card');
    cardElement.click();

    expect(component.characterSelect.emit).toHaveBeenCalledWith(mockCharacter);
  });

  it('should not emit characterSelect when not selectable', () => {
    component.selectable = false;
    spyOn(component.characterSelect, 'emit');

    const cardElement = fixture.nativeElement.querySelector('.character-card');
    cardElement.click();

    expect(component.characterSelect.emit).not.toHaveBeenCalled();
  });

  it('should emit characterEdit when edit button clicked', () => {
    component.showActions = true;
    fixture.detectChanges();
    spyOn(component.characterEdit, 'emit');

    const editButton = fixture.nativeElement.querySelector('.btn-icon-small.edit');
    editButton.click();

    expect(component.characterEdit.emit).toHaveBeenCalledWith(mockCharacter);
  });

  it('should emit characterDelete when delete button clicked', () => {
    component.showActions = true;
    fixture.detectChanges();
    spyOn(component.characterDelete, 'emit');

    const deleteButton = fixture.nativeElement.querySelector('.btn-icon-small.delete');
    deleteButton.click();

    expect(component.characterDelete.emit).toHaveBeenCalledWith(mockCharacter);
  });

  it('should show background when showBackground is true', () => {
    component.showBackground = true;
    fixture.detectChanges();

    const backgroundElement = fixture.nativeElement.querySelector('.character-background');
    expect(backgroundElement).toBeTruthy();
    expect(backgroundElement.textContent).toContain('Test background');
  });

  it('should hide background when showBackground is false', () => {
    component.showBackground = false;
    fixture.detectChanges();

    const backgroundElement = fixture.nativeElement.querySelector('.character-background');
    expect(backgroundElement).toBeFalsy();
  });
});
