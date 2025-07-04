import { Component, Input, Output, EventEmitter, OnInit } from '@angular/core';
import { Character, CharacterClass, Race } from '../../models/character.model';
import { CharacterDisplayService } from '../../services/character-display.service';

/**
 * D&D-themed character creation and editing form component
 */
@Component({
  selector: 'app-character-form',
  templateUrl: './character-form.component.html',
  styleUrls: ['./character-form.component.css'],
  standalone: false
})
export class CharacterFormComponent implements OnInit {
  @Input() character: Character | null = null;
  @Input() isEditing: boolean = false;
  @Input() loading: boolean = false;
  @Output() save = new EventEmitter<Character>();
  @Output() cancel = new EventEmitter<void>();

  form: Character = {
    name: '',
    class: '',
    race: '',
    background: ''
  };

  // D&D Character Classes with thematic icons
  characterClasses: CharacterClass[] = [];

  // D&D Races with thematic icons
  races: Race[] = [];

  constructor(private characterDisplayService: CharacterDisplayService) {
    this.initializeOptions();
  }

  ngOnInit(): void {
    if (this.character) {
      this.form = { ...this.character };
    }
  }

  /**
   * Initialize character classes and races from the service
   */
  private initializeOptions(): void {
    // Initialize character classes with descriptions
    this.characterClasses = [
      { id: 'barbarian', name: 'Barbarian', icon: '⚔️', description: 'A fierce warrior born of the wilderness' },
      { id: 'bard', name: 'Bard', icon: '🎵', description: 'A master of song, speech, and the magic they contain' },
      { id: 'cleric', name: 'Cleric', icon: '⛪', description: 'A priestly champion who wields divine magic' },
      { id: 'druid', name: 'Druid', icon: '🌿', description: 'A priest of nature, wielding elemental forces' },
      { id: 'fighter', name: 'Fighter', icon: '🛡️', description: 'A master of martial combat, skilled with weapons' },
      { id: 'monk', name: 'Monk', icon: '👊', description: 'A master of martial arts, harnessing inner power' },
      { id: 'paladin', name: 'Paladin', icon: '⚡', description: 'A holy warrior bound to a sacred oath' },
      { id: 'ranger', name: 'Ranger', icon: '🏹', description: 'A warrior of the wilderness, hunter and tracker' },
      { id: 'rogue', name: 'Rogue', icon: '🗡️', description: 'A scoundrel who uses stealth and trickery' },
      { id: 'sorcerer', name: 'Sorcerer', icon: '🔥', description: 'A spellcaster who draws on inherent magic' },
      { id: 'warlock', name: 'Warlock', icon: '👁️', description: 'A wielder of magic derived from a bargain' },
      { id: 'wizard', name: 'Wizard', icon: '🧙', description: 'A scholarly magic-user capable of great power' }
    ];

    // Initialize races with descriptions and traits
    this.races = [
      {
        id: 'human',
        name: 'Human',
        icon: '👤',
        description: 'Versatile and ambitious',
        traits: ['Extra skill', 'Bonus feat', 'Adaptable']
      },
      {
        id: 'elf',
        name: 'Elf',
        icon: '🧝',
        description: 'Graceful and magical',
        traits: ['Darkvision', 'Keen senses', 'Fey ancestry']
      },
      {
        id: 'dwarf',
        name: 'Dwarf',
        icon: '⛏️',
        description: 'Hardy and resilient',
        traits: ['Darkvision', 'Dwarven resilience', 'Stonecunning']
      },
      {
        id: 'halfling',
        name: 'Halfling',
        icon: '🍃',
        description: 'Small but brave',
        traits: ['Lucky', 'Brave', 'Halfling nimbleness']
      },
      {
        id: 'dragonborn',
        name: 'Dragonborn',
        icon: '🐉',
        description: 'Descendants of dragons',
        traits: ['Draconic ancestry', 'Breath weapon', 'Damage resistance']
      },
      {
        id: 'gnome',
        name: 'Gnome',
        icon: '🧚',
        description: 'Small and clever',
        traits: ['Darkvision', 'Gnome cunning', 'Tinker']
      },
      {
        id: 'half-elf',
        name: 'Half-Elf',
        icon: '🌗',
        description: 'Between two worlds',
        traits: ['Darkvision', 'Fey ancestry', 'Two skills']
      },
      {
        id: 'half-orc',
        name: 'Half-Orc',
        icon: '👹',
        description: 'Strong and fierce',
        traits: ['Darkvision', 'Relentless endurance', 'Savage attacks']
      },
      {
        id: 'tiefling',
        name: 'Tiefling',
        icon: '😈',
        description: 'Touched by the infernal',
        traits: ['Darkvision', 'Hellish resistance', 'Infernal legacy']
      }
    ];
  }

  /**
   * Get character class details by ID
   */
  getCharacterClass(classId: string): CharacterClass | undefined {
    return this.characterClasses.find(c => c.id === classId);
  }

  /**
   * Get race details by ID
   */
  getRace(raceId: string): Race | undefined {
    return this.races.find(r => r.id === raceId);
  }

  /**
   * Handle form submission
   */
  onSubmit(): void {
    if (this.isFormValid()) {
      this.save.emit({ ...this.form });
    }
  }

  /**
   * Handle cancel action
   */
  onCancel(): void {
    this.cancel.emit();
  }

  /**
   * Reset form to initial state
   */
  resetForm(): void {
    this.form = {
      name: '',
      class: '',
      race: '',
      background: ''
    };
  }

  /**
   * Check if form is valid
   */
  isFormValid(): boolean {
    return !!(this.form.name.trim() && this.form.class && this.form.race);
  }

  /**
   * Generate placeholder text for background based on selected class and race
   */  getBackgroundPlaceholder(): string {
    const selectedClass = this.getCharacterClass(this.form.class);
    const selectedRace = this.getRace(this.form.race);

    if (selectedClass && selectedRace) {
      return `Tell the story of ${this.form.name || 'your character'}, the ${selectedRace.name} ${selectedClass.name}...`;
    }
    return 'Describe your character\'s background, history, and motivations...';
  }
}
