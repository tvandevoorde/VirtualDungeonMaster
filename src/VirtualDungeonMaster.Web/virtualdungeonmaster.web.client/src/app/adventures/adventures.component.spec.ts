import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';
import { ActivatedRoute, Router } from '@angular/router';
import { of, throwError } from 'rxjs';
import { FormsModule } from '@angular/forms';

import { AdventuresComponent } from './adventures.component';
import { CharacterService } from '../services/character.service';
import { AdventureService } from '../services/adventure.service';
import { AdventureSession, AdventureSessionSummary, AdventureStatus } from '../models/adventure.model';
import { CharacterCardComponent } from '../components/character-card/character-card.component';
import { LoadingComponent } from '../components/loading/loading.component';
import { ErrorMessageComponent } from '../components/error-message/error-message.component';
import { Character } from '../models/character.model';

describe('AdventuresComponent', () => {
  let component: AdventuresComponent;
  let fixture: ComponentFixture<AdventuresComponent>;
  let mockCharacterService: jasmine.SpyObj<CharacterService>;
  let mockAdventureService: jasmine.SpyObj<AdventureService>;
  let mockActivatedRoute: any;
  let mockRouter: jasmine.SpyObj<Router>;

  const mockCharacter: Character = {
    id: 1,
    name: 'Test Hero',
    class: 'fighter',
    race: 'human',
    background: 'Test background'
  };

  const mockAdventure: AdventureSession = {
    id: 1,
    characterId: 1,
    title: 'Test Adventure',
    status: 'Active',
    currentTurnNumber: 1,
    currentPrompt: 'Test prompt',
    startedAt: '2023-01-01T10:00:00Z'
  };

  const mockAdventureSummary: AdventureSessionSummary = {
    id: 1,
    characterId: 1,
    title: 'Test Adventure',
    status: 'Active',
    currentTurnNumber: 1,
    startedAt: '2023-01-01T10:00:00Z'
  };

  beforeEach(async () => {
    const characterSpy = jasmine.createSpyObj('CharacterService', ['getCharacters', 'getCharacter']);
    const adventureSpy = jasmine.createSpyObj('AdventureService', [
      'getCharacterSessions',
      'getSession',
      'getSessionEvents',
      'startSession',
      'submitTurn',
      'endSession'
    ]);
    const routerSpy = jasmine.createSpyObj('Router', ['navigate']);

    mockActivatedRoute = {
      snapshot: {
        paramMap: {
          get: jasmine.createSpy('get').and.returnValue(null)
        }
      }
    };

    await TestBed.configureTestingModule({
      declarations: [
        AdventuresComponent,
        CharacterCardComponent,
        LoadingComponent,
        ErrorMessageComponent
      ],
      imports: [RouterTestingModule, FormsModule],
      providers: [
        { provide: CharacterService, useValue: characterSpy },
        { provide: AdventureService, useValue: adventureSpy },
        { provide: ActivatedRoute, useValue: mockActivatedRoute },
        { provide: Router, useValue: routerSpy }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(AdventuresComponent);
    component = fixture.componentInstance;
    mockCharacterService = TestBed.inject(CharacterService) as jasmine.SpyObj<CharacterService>;
    mockAdventureService = TestBed.inject(AdventureService) as jasmine.SpyObj<AdventureService>;
    mockRouter = TestBed.inject(Router) as jasmine.SpyObj<Router>;

    // Setup default mock returns
    mockCharacterService.getCharacters.and.returnValue(of([mockCharacter]));
    mockCharacterService.getCharacter.and.returnValue(of(mockCharacter));
    mockAdventureService.getCharacterSessions.and.returnValue(of([mockAdventureSummary]));
    mockAdventureService.getSession.and.returnValue(of(mockAdventure));
    mockAdventureService.getSessionEvents.and.returnValue(of([]));
    mockAdventureService.startSession.and.returnValue(of(mockAdventure));
    mockAdventureService.submitTurn.and.returnValue(of({
      id: 1,
      turnNumber: 2,
      timestamp: '2023-01-01T10:30:00Z',
      playerInput: 'Test input',
      aiResponse: 'Test response'
    }));
    mockAdventureService.endSession.and.returnValue(of(void 0));

    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should load characters on init when no adventure ID in route', () => {
    expect(mockCharacterService.getCharacters).toHaveBeenCalled();
    expect(component.characters.length).toBe(1);
  });

  it('should load adventure session when ID is in route', () => {
    // Reset component and set up route parameter
    mockActivatedRoute.snapshot.paramMap.get.and.returnValue('1');

    component.ngOnInit();

    expect(mockAdventureService.getSession).toHaveBeenCalledWith(1);
  });

  it('should select character and load adventures', () => {
    component.selectCharacter(mockCharacter);

    expect(component.selectedCharacter).toBe(mockCharacter);
    expect(mockAdventureService.getCharacterSessions).toHaveBeenCalledWith(1);
  });

  it('should start creating adventure', () => {
    component.startCreateAdventure();

    expect(component.creating).toBe(true);
    expect(component.newAdventureTitle).toBe('');
  });

  it('should create adventure', () => {
    component.selectedCharacter = mockCharacter;
    component.newAdventureTitle = 'New Adventure';

    component.createAdventure();

    expect(mockAdventureService.startSession).toHaveBeenCalledWith({
      characterId: 1,
      title: 'New Adventure'
    });
  });

  it('should navigate to adventure', () => {
    component.goToAdventure(mockAdventure);

    expect(mockRouter.navigate).toHaveBeenCalledWith(['/adventures', 1]);
  });

  it('should check if adventure is active', () => {
    component.currentAdventure = mockAdventure;
    expect(component.isAdventureActive()).toBe(true);

    component.currentAdventure = { ...mockAdventure, status: AdventureStatus.Completed };
    expect(component.isAdventureActive()).toBe(false);
  });

  it('should clear error', () => {
    component.error = 'Test error';
    component.clearError();
    expect(component.error).toBe('');
  });

  it('should handle loading errors', () => {
    mockCharacterService.getCharacters.and.returnValue(throwError('Load error'));

    component.loadCharacters();

    expect(component.error).toBe('Failed to load characters.');
    expect(component.loading).toBe(false);
  });

  it('should navigate back to adventure list', () => {
    component.currentAdventure = mockAdventure;

    component.backToAdventureList();

    expect(component.currentAdventure).toBeNull();
    expect(mockRouter.navigate).toHaveBeenCalledWith(['/adventures']);
  });
});
