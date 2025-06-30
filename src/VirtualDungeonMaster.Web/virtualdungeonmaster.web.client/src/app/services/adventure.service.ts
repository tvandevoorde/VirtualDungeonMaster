import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdventureSession, NarrativeEvent, StartSessionRequest, SubmitTurnRequest, AdventureStatus } from '../models/adventure.model';

/**
 * Service for managing adventure sessions and gameplay
 */
@Injectable({
  providedIn: 'root'
})
export class AdventureService {
  private readonly baseUrl = '/api/Adventures';

  constructor(private http: HttpClient) {}

  /**
   * Get adventure sessions for a character
   */
  getCharacterSessions(characterId: number): Observable<AdventureSession[]> {
    return this.http.get<AdventureSession[]>(`${this.baseUrl}/character/${characterId}/sessions`);
  }

  /**
   * Get a specific adventure session
   */
  getSession(sessionId: number): Observable<AdventureSession> {
    return this.http.get<AdventureSession>(`${this.baseUrl}/session/${sessionId}`);
  }

  /**
   * Start a new adventure session
   */
  startSession(request: StartSessionRequest): Observable<AdventureSession> {
    return this.http.post<AdventureSession>(`${this.baseUrl}/session`, request);
  }

  /**
   * Submit a turn in an adventure session
   */
  submitTurn(sessionId: number, request: SubmitTurnRequest): Observable<NarrativeEvent> {
    return this.http.post<NarrativeEvent>(`${this.baseUrl}/session/${sessionId}/turn`, request);
  }

  /**
   * End an adventure session
   */
  endSession(sessionId: number): Observable<void> {
    return this.http.post<void>(`${this.baseUrl}/session/${sessionId}/end`, {});
  }
}
