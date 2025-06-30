import { Component, Input, Output, EventEmitter } from '@angular/core';
import { Character } from '../../models/character.model';
import { CharacterDisplayService } from '../../services/character-display.service';

@Component({
  selector: 'app-character-card',
  templateUrl: './character-card.component.html',
  styleUrls: ['./character-card.component.css'],
  standalone: false
})
export class CharacterCardComponent {
  @Input() character!: Character;
  @Input() selectable = false;
  @Input() showActions = false;
  @Input() showBackground = false;
  @Input() loading = false;

  @Output() characterSelect = new EventEmitter<Character>();
  @Output() characterEdit = new EventEmitter<Character>();
  @Output() characterDelete = new EventEmitter<Character>();

  constructor(private characterDisplayService: CharacterDisplayService) {}

  /**
   * Get character class display info
   */
  getClassInfo() {
    return this.characterDisplayService.getClassInfo(this.character.class);
  }

  /**
   * Get race display info
   */
  getRaceInfo() {
    return this.characterDisplayService.getRaceInfo(this.character.race);
  }

  /**
   * Handle character selection
   */
  onSelect(): void {
    if (this.selectable) {
      this.characterSelect.emit(this.character);
    }
  }

  /**
   * Handle character edit
   */
  onEdit(): void {
    this.characterEdit.emit(this.character);
  }

  /**
   * Handle character delete
   */
  onDelete(): void {
    this.characterDelete.emit(this.character);
  }
}
