export interface AdventureSession {
  id?: number;
  characterId: number;
  title: string;
  status: string; // 'Active' | 'Completed'
  startedAt: string;
  endedAt?: string;
  currentTurnNumber: number;
  currentPrompt: string;
  adventureSummary: string;
}

export interface AdventureSessionSummary {
  id?: number;
  characterId: number;
  title: string;
  status: string; // 'Active' | 'Completed'
  startedAt: string;
  endedAt?: string;
  currentTurnNumber: number;
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

// Keep enum for backwards compatibility but use string values
export enum AdventureStatus {
  Active = 'Active',
  Completed = 'Completed'
}
