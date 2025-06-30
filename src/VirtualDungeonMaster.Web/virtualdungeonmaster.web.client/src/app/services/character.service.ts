import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Character } from '../models/character.model';

/**
 * Service for managing character data and API calls
 * Follows Angular best practices by centralizing HTTP operations
 */
@Injectable({
  providedIn: 'root'
})
export class CharacterService {
  private readonly baseUrl = '/api/Characters';

  constructor(private http: HttpClient) {}

  /**
   * Get all characters
   */
  getCharacters(): Observable<Character[]> {
    return this.http.get<Character[]>(this.baseUrl);
  }

  /**
   * Get a character by ID
   */
  getCharacter(id: number): Observable<Character> {
    return this.http.get<Character>(`${this.baseUrl}/${id}`);
  }

  /**
   * Create a new character
   */
  createCharacter(character: Character): Observable<Character> {
    return this.http.post<Character>(this.baseUrl, character);
  }

  /**
   * Update an existing character
   */
  updateCharacter(id: number, character: Character): Observable<Character> {
    return this.http.put<Character>(`${this.baseUrl}/${id}`, character);
  }

  /**
   * Delete a character
   */
  deleteCharacter(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
