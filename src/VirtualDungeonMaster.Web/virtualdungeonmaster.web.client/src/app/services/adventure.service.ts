import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AdventureSession, AdventureSessionSummary, NarrativeEvent, StartSessionRequest, SubmitTurnRequest, AdventureStatus } from '../models/adventure.model';

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
   * Get adventure session summaries for a character
   */
  getCharacterSessions(characterId: number): Observable<AdventureSessionSummary[]> {
    return this.http.get<AdventureSessionSummary[]>(`${this.baseUrl}/character/${characterId}/sessions`);
  }

  /**
   * Get a specific adventure session
   */
  getSession(sessionId: number): Observable<AdventureSession> {
    return this.http.get<AdventureSession>(`${this.baseUrl}/session/${sessionId}`);
  }

  /**
   * Get events for a specific adventure session with pagination
   */
  getSessionEvents(sessionId: number, skip: number = 0, take: number = 20): Observable<NarrativeEvent[]> {
    const params = new HttpParams()
      .set('skip', skip.toString())
      .set('take', take.toString());

    return this.http.get<NarrativeEvent[]>(`${this.baseUrl}/session/${sessionId}/events`, { params });
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
