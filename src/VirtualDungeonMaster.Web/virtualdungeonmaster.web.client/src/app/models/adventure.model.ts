export interface AdventureSession {
  id?: number;
  characterId: number;
  title: string;
  status: AdventureStatus;
  startedAt: string;
  endedAt?: string;
  events: NarrativeEvent[];
  currentTurnNumber: number;
  currentPrompt: string;
}

export interface NarrativeEvent {
  id?: number;
  turnNumber: number;
  playerInput?: string;
  aiResponse?: string;
  timestamp: string;
}

export interface StartSessionRequest {
  characterId: number;
  title?: string;
}

export interface SubmitTurnRequest {
  playerInput: string;
}

export enum AdventureStatus {
  Active = 0,
  Completed = 1,
  Paused = 2
}
