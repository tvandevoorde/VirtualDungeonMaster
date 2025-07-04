import { Component, OnInit, ViewChild, ElementRef, HostListener } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { Character } from '../models/character.model';
import { CharacterService } from '../services/character.service';
import { AdventureService } from '../services/adventure.service';
import { AdventureSession, AdventureSessionSummary, NarrativeEvent, AdventureStatus } from '../models/adventure.model';

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
  adventures: AdventureSessionSummary[] = [];
  currentAdventure: AdventureSession | null = null;
  events: NarrativeEvent[] = [];
  loading = false;
  loadingEvents = false;
  error = '';
  creating = false;
  newAdventureTitle = '';

  // Pagination for events
  eventSkip = 0;
  eventTake = 20;
  hasMoreEvents = true;
  isLoadingMore = false;

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
      next: (data: AdventureSessionSummary[]) => {
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

  goToAdventure(adventure: AdventureSessionSummary) {
    this.router.navigate(['/adventures', adventure.id]);
  }

  loadAdventureSession(sessionId: number) {
    this.loading = true;
    this.currentAdventure = null;
    this.events = [];
    this.eventSkip = 0;
    this.hasMoreEvents = true;

    this.adventureService.getSession(sessionId).subscribe({
      next: (session: AdventureSession) => {
        this.currentAdventure = session;
        // Also load the character for display
        this.characterService.getCharacter(session.characterId).subscribe({
          next: (character: Character) => {
            this.selectedCharacter = character;
            this.loading = false;

            // Load initial events
            this.loadEvents(sessionId);
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

  loadEvents(sessionId: number, loadMore: boolean = false) {
    if (this.isLoadingMore || !this.hasMoreEvents && loadMore) return;

    if (loadMore) {
      this.isLoadingMore = true;
    } else {
      this.loadingEvents = true;
    }

    this.adventureService.getSessionEvents(sessionId, this.eventSkip, this.eventTake).subscribe({
      next: (events: NarrativeEvent[]) => {
        if (events.length < this.eventTake) {
          this.hasMoreEvents = false;
        }

        if (loadMore) {
          // When loading more (older events), add them to the bottom
          // Backend returns descending order, so older events go at the end
          this.events.push(...events);
        } else {
          // When loading initial events, keep backend order (newest first, descending turn order)
          this.events = events;
        }

        this.eventSkip += events.length;
        this.loadingEvents = false;
        this.isLoadingMore = false;

        // Note: Removed automatic scroll to latest turn to let user control scroll position
      },
      error: () => {
        this.error = 'Failed to load adventure events.';
        this.loadingEvents = false;
        this.isLoadingMore = false;
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
        // Add the new event to the top of the events array
        this.events.unshift(narrativeEvent);
        this.currentAdventure!.currentTurnNumber = narrativeEvent.turnNumber;
        this.playerInput = '';
        this.submittingTurn = false;

        // Note: Removed automatic scroll to let user control scroll position
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
        this.currentAdventure!.status = 'Completed';
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
    return this.currentAdventure?.status === 'Active';
  }

  /**
   * Clear error message
   */
  clearError(): void {
    this.error = '';
  }

  @HostListener('scroll', ['$event'])
  onScroll(event: any) {
    // This is for global scroll events if needed
  }

  onAdventureLogScroll(event: any) {
    // Check if we're scrolling in the adventure log
    if (!this.currentAdventure) return;

    const element = event.target;
    const scrollTop = element.scrollTop;
    const scrollHeight = element.scrollHeight;
    const clientHeight = element.clientHeight;

    // Load more events when scrolling near the bottom (for older events)
    if (scrollTop + clientHeight >= scrollHeight - 100 && this.hasMoreEvents && !this.isLoadingMore) {
      this.loadEvents(this.currentAdventure.id!, true);
    }
  }
}
