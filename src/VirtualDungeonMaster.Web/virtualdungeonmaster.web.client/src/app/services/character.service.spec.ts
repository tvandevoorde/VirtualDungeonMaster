import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { CharacterService } from './character.service';
import { Character } from '../models/character.model';

describe('CharacterService', () => {
  let service: CharacterService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [CharacterService]
    });
    service = TestBed.inject(CharacterService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should get all characters', () => {
    const mockCharacters: Character[] = [
      { id: 1, name: 'Test Hero', class: 'fighter', race: 'human', background: 'Test' }
    ];

    service.getCharacters().subscribe(characters => {
      expect(characters).toEqual(mockCharacters);
    });

    const req = httpMock.expectOne('/api/Characters');
    expect(req.request.method).toBe('GET');
    req.flush(mockCharacters);
  });

  it('should get a character by ID', () => {
    const mockCharacter: Character = {
      id: 1,
      name: 'Test Hero',
      class: 'fighter',
      race: 'human',
      background: 'Test'
    };

    service.getCharacter(1).subscribe(character => {
      expect(character).toEqual(mockCharacter);
    });

    const req = httpMock.expectOne('/api/Characters/1');
    expect(req.request.method).toBe('GET');
    req.flush(mockCharacter);
  });

  it('should create a character', () => {
    const newCharacter: Character = {
      name: 'New Hero',
      class: 'wizard',
      race: 'elf',
      background: 'Scholarly'
    };

    const createdCharacter: Character = { ...newCharacter, id: 1 };

    service.createCharacter(newCharacter).subscribe(character => {
      expect(character).toEqual(createdCharacter);
    });

    const req = httpMock.expectOne('/api/Characters');
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(newCharacter);
    req.flush(createdCharacter);
  });

  it('should update a character', () => {
    const updatedCharacter: Character = {
      id: 1,
      name: 'Updated Hero',
      class: 'paladin',
      race: 'human',
      background: 'Noble'
    };

    service.updateCharacter(1, updatedCharacter).subscribe(character => {
      expect(character).toEqual(updatedCharacter);
    });

    const req = httpMock.expectOne('/api/Characters/1');
    expect(req.request.method).toBe('PUT');
    expect(req.request.body).toEqual(updatedCharacter);
    req.flush(updatedCharacter);
  });

  it('should delete a character', () => {
    service.deleteCharacter(1).subscribe(response => {
      expect(response).toBeNull();
    });

    const req = httpMock.expectOne('/api/Characters/1');
    expect(req.request.method).toBe('DELETE');
    req.flush(null);
  });
});
