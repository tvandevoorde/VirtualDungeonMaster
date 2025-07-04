export interface Character {
  id?: number;
  name: string;
  class: string;
  race: string;
  background: string;
}

export interface CharacterClass {
  id: string;
  name: string;
  icon: string;
  description: string;
}

export interface Race {
  id: string;
  name: string;
  icon: string;
  description: string;
  traits: string[];
}

// Display-related interfaces for character information
export interface ClassInfo {
  name: string;
  icon: string;
}

export interface RaceInfo {
  name: string;
  icon: string;
}
