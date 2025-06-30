import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Character } from '../models/character.model';
import { CharacterService } from '../services/character.service';
import { AdventureService } from '../services/adventure.service';
import { AdventureSession, NarrativeEvent, AdventureStatus } from '../models/adventure.model';

@Component({
  selector: 'app-adventures',
  templateUrl: './adventures.component.html',
  styleUrls: ['./adventures.component.css'],
  standalone: false
})
export class AdventuresComponent implements OnInit {
  @ViewChild('adventureLog', { static: false }) adventureLog!: ElementRef;

  characters: Character[] = [];
  selectedCharacter: Character | null = null;
  adventures: AdventureSession[] = [];
  currentAdventure: AdventureSession | null = null;
  loading = false;
  error = '';
  creating = false;
  newAdventureTitle = '';

  // Turn submission
  playerInput = '';
  submittingTurn = false;

  constructor(
    private characterService: CharacterService,
    private adventureService: AdventureService,
    private router: Router,
    private route: ActivatedRoute
  ) {}

  ngOnInit() {
    // Check if we have an adventure ID in the route
    const adventureId = this.route.snapshot.paramMap.get('id');
    if (adventureId) {
      this.loadAdventureSession(Number(adventureId));
    } else {
      this.loadCharacters();
    }
  }

  loadCharacters() {
    this.loading = true;
    this.characterService.getCharacters().subscribe({
      next: (data: Character[]) => {
        this.characters = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load characters.';
        this.loading = false;
      }
    });
  }

  selectCharacter(character: Character) {
    this.selectedCharacter = character;
    this.loadAdventures(character.id!);
  }

  loadAdventures(characterId: number) {
    this.loading = true;
    this.adventureService.getCharacterSessions(characterId).subscribe({
      next: (data: AdventureSession[]) => {
        this.adventures = data;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to load adventures.';
        this.loading = false;
      }
    });
  }

  startCreateAdventure() {
    this.creating = true;
    this.newAdventureTitle = '';
  }

  createAdventure() {
    if (!this.selectedCharacter || !this.newAdventureTitle.trim()) return;
    this.loading = true;
    this.adventureService.startSession({
      characterId: this.selectedCharacter.id!,
      title: this.newAdventureTitle
    }).subscribe({
      next: (session: AdventureSession) => {
        this.router.navigate(['/adventures', session.id]);
      },
      error: () => {
        this.error = 'Failed to create adventure.';
        this.loading = false;
      }
    });
  }

  goToAdventure(adventure: AdventureSession) {
    this.router.navigate(['/adventures', adventure.id]);
  }

  loadAdventureSession(sessionId: number) {
    this.loading = true;
    this.adventureService.getSession(sessionId).subscribe({
      next: (session: AdventureSession) => {
        this.currentAdventure = session;
        // Also load the character for display
        this.characterService.getCharacter(session.characterId).subscribe({
          next: (character: Character) => {
            this.selectedCharacter = character;
            this.loading = false;

            // Scroll to latest turn after the view is initialized
            this.scrollToLatestTurn();
          },
          error: () => {
            this.error = 'Failed to load character details.';
            this.loading = false;
          }
        });
      },
      error: () => {
        this.error = 'Failed to load adventure session.';
        this.loading = false;
      }
    });
  }

  submitTurn() {
    if (!this.currentAdventure || !this.currentAdventure.id || !this.playerInput.trim()) return;

    this.submittingTurn = true;
    this.adventureService.submitTurn(this.currentAdventure.id, {
      playerInput: this.playerInput.trim()
    }).subscribe({
      next: (narrativeEvent: NarrativeEvent) => {
        // Add the new event to the current adventure
        this.currentAdventure!.events.push(narrativeEvent);
        this.currentAdventure!.currentTurnNumber = narrativeEvent.turnNumber;
        this.playerInput = '';
        this.submittingTurn = false;

        // Scroll to show the latest turn at the top
        this.scrollToLatestTurn();
      },
      error: () => {
        this.error = 'Failed to submit turn.';
        this.submittingTurn = false;
      }
    });
  }

  endAdventure() {
    if (!this.currentAdventure || !this.currentAdventure.id) return;

    this.loading = true;
    this.adventureService.endSession(this.currentAdventure.id).subscribe({
      next: () => {
        this.currentAdventure!.status = AdventureStatus.Completed;
        this.loading = false;
      },
      error: () => {
        this.error = 'Failed to end adventure.';
        this.loading = false;
      }
    });
  }

  backToAdventureList() {
    this.currentAdventure = null;
    this.router.navigate(['/adventures']);
  }

  isAdventureActive(): boolean {
    return this.currentAdventure?.status === AdventureStatus.Active;
  }

  /**
   * Scroll the adventure log so the latest turn appears at the top
   */
  private scrollToLatestTurn() {
    // Use setTimeout to ensure DOM has updated after the new event is added
    setTimeout(() => {
      if (this.adventureLog && this.adventureLog.nativeElement) {
        const logElement = this.adventureLog.nativeElement;

        // Try to find the last event group (most recent turn)
        const lastEventGroup = logElement.querySelector('.event-group:last-child');

        if (lastEventGroup) {
          // Scroll so the last event group appears at the top of the visible area
          lastEventGroup.scrollIntoView({
            behavior: 'smooth',
            block: 'start'
          });
        } else {
          // If no event groups yet, just scroll to the bottom to show the latest content
          logElement.scrollTo({
            top: logElement.scrollHeight,
            behavior: 'smooth'
          });
        }
      }
    }, 200); // Increased timeout to ensure DOM is fully updated
  }

  /**
   * Clear error message
   */
  clearError(): void {
    this.error = '';
  }
}
