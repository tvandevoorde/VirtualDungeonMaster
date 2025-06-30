import { Component, OnInit } from '@angular/core';
import { Character } from '../models/character.model';
import { CharacterService } from '../services/character.service';

@Component({
  selector: 'app-characters',
  templateUrl: './characters.component.html',
  styleUrls: ['./characters.component.css'],
  standalone: false
})
export class CharactersComponent implements OnInit {
  characters: Character[] = [];
  showForm: boolean = false;
  editingCharacter: Character | null = null;
  loading = false;
  error = '';

  constructor(private characterService: CharacterService) {}

  ngOnInit(): void {
    this.loadCharacters();
  }

  loadCharacters() {
    this.loading = true;
    this.characterService.getCharacters().subscribe({
      next: (data: Character[]) => {
        this.characters = data;
        this.loading = false;
      },
      error: (err: any) => {
        this.error = 'Failed to load characters.';
        this.loading = false;
      }
    });
  }

  /**
   * Show the character creation form
   */
  showCreateForm(): void {
    this.editingCharacter = null;
    this.showForm = true;
    this.error = '';
  }

  /**
   * Show the character editing form
   */
  editCharacter(character: Character): void {
    this.editingCharacter = character;
    this.showForm = true;
    this.error = '';
  }

  /**
   * Handle character creation/update
   */
  onSaveCharacter(character: Character): void {
    if (this.editingCharacter) {
      this.updateCharacter({ ...character, id: this.editingCharacter.id });
    } else {
      this.addCharacter(character);
    }
  }

  /**
   * Handle form cancellation
   */
  onCancelForm(): void {
    this.showForm = false;
    this.editingCharacter = null;
    this.error = '';
  }

  /**
   * Create a new character
   */
  addCharacter(character: Character): void {
    this.loading = true;
    this.characterService.createCharacter(character).subscribe({
      next: (newCharacter: Character) => {
        this.characters.push(newCharacter);
        this.showForm = false;
        this.loading = false;
        this.error = '';
      },
      error: (err: any) => {
        this.error = 'Failed to add character.';
        this.loading = false;
      }
    });
  }

  /**
   * Update an existing character
   */
  updateCharacter(character: Character): void {
    this.loading = true;
    this.characterService.updateCharacter(character.id!, character).subscribe({
      next: (updated: Character) => {
        const index = this.characters.findIndex(c => c.id === character.id);
        if (index !== -1) {
          this.characters[index] = updated;
        }
        this.showForm = false;
        this.loading = false;
        this.error = '';
      },
      error: (err: any) => {
        this.error = 'Failed to update character.';
        this.loading = false;
      }
    });
  }

  /**
   * Delete a character
   */
  deleteCharacter(character: Character): void {
    if (confirm(`Are you sure you want to delete ${character.name}?`)) {
      this.loading = true;
      this.characterService.deleteCharacter(character.id!).subscribe({
        next: () => {
          this.characters = this.characters.filter(c => c.id !== character.id);
          this.loading = false;
          this.error = '';
        },
        error: (err: any) => {
          this.error = 'Failed to delete character.';
          this.loading = false;
        }
      });
    }
  }

  /**
   * Clear error message
   */
  clearError(): void {
    this.error = '';
  }
}
