import { Injectable } from '@angular/core';
import { ClassInfo, RaceInfo } from '../models/character.model';

@Injectable({
  providedIn: 'root'
})
export class CharacterDisplayService {

  private readonly classes: Record<string, ClassInfo> = {
    'barbarian': { name: 'Barbarian', icon: '⚔️' },
    'bard': { name: 'Bard', icon: '🎵' },
    'cleric': { name: 'Cleric', icon: '⛪' },
    'druid': { name: 'Druid', icon: '🌿' },
    'fighter': { name: 'Fighter', icon: '🛡️' },
    'monk': { name: 'Monk', icon: '👊' },
    'paladin': { name: 'Paladin', icon: '⚡' },
    'ranger': { name: 'Ranger', icon: '🏹' },
    'rogue': { name: 'Rogue', icon: '🗡️' },
    'sorcerer': { name: 'Sorcerer', icon: '🔥' },
    'warlock': { name: 'Warlock', icon: '👁️' },
    'wizard': { name: 'Wizard', icon: '🧙' }
  };

  private readonly races: Record<string, RaceInfo> = {
    'human': { name: 'Human', icon: '👤' },
    'elf': { name: 'Elf', icon: '🧝' },
    'dwarf': { name: 'Dwarf', icon: '⛏️' },
    'halfling': { name: 'Halfling', icon: '🍃' },
    'dragonborn': { name: 'Dragonborn', icon: '🐉' },
    'gnome': { name: 'Gnome', icon: '🧚' },
    'half-elf': { name: 'Half-Elf', icon: '🌗' },
    'half-orc': { name: 'Half-Orc', icon: '👹' },
    'tiefling': { name: 'Tiefling', icon: '😈' }
  };

  /**
   * Get character class display information
   */
  getClassInfo(classId: string): ClassInfo {
    return this.classes[classId] || { name: classId, icon: '❓' };
  }

  /**
   * Get race display information
   */
  getRaceInfo(raceId: string): RaceInfo {
    return this.races[raceId] || { name: raceId, icon: '❓' };
  }

  /**
   * Get all available classes
   */
  getAvailableClasses(): { id: string; info: ClassInfo }[] {
    return Object.entries(this.classes).map(([id, info]) => ({ id, info }));
  }

  /**
   * Get all available races
   */
  getAvailableRaces(): { id: string; info: RaceInfo }[] {
    return Object.entries(this.races).map(([id, info]) => ({ id, info }));
  }
}
