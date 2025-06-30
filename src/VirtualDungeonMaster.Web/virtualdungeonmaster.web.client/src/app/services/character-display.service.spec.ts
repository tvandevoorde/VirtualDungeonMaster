import { TestBed } from '@angular/core/testing';
import { CharacterDisplayService } from './character-display.service';

describe('CharacterDisplayService', () => {
  let service: CharacterDisplayService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CharacterDisplayService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should return correct class info for barbarian', () => {
    const classInfo = service.getClassInfo('barbarian');
    expect(classInfo.name).toBe('Barbarian');
    expect(classInfo.icon).toBe('⚔️');
  });

  it('should return correct race info for elf', () => {
    const raceInfo = service.getRaceInfo('elf');
    expect(raceInfo.name).toBe('Elf');
    expect(raceInfo.icon).toBe('🧝');
  });

  it('should return fallback info for unknown class', () => {
    const classInfo = service.getClassInfo('unknown');
    expect(classInfo.name).toBe('unknown');
    expect(classInfo.icon).toBe('❓');
  });

  it('should return fallback info for unknown race', () => {
    const raceInfo = service.getRaceInfo('unknown');
    expect(raceInfo.name).toBe('unknown');
    expect(raceInfo.icon).toBe('❓');
  });

  it('should return all available classes', () => {
    const classes = service.getAvailableClasses();
    expect(classes.length).toBeGreaterThan(0);
    expect(classes[0].id).toBeDefined();
    expect(classes[0].info).toBeDefined();
  });

  it('should return all available races', () => {
    const races = service.getAvailableRaces();
    expect(races.length).toBeGreaterThan(0);
    expect(races[0].id).toBeDefined();
    expect(races[0].info).toBeDefined();
  });
});
