import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CharactersComponent } from './characters/characters.component';
import { AdventuresComponent } from './adventures/adventures.component';

const routes: Routes = [
  { path: 'characters', component: CharactersComponent },
  { path: 'adventures', component: AdventuresComponent },
  { path: 'adventures/:id', component: AdventuresComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
