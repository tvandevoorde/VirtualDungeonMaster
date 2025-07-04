import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CharactersComponent } from './characters/characters.component';
import { AdventuresComponent } from './adventures/adventures.component';
import { CharacterFormComponent } from './components/character-form/character-form.component';
import { CharacterCardComponent } from './components/character-card/character-card.component';
import { LoadingComponent } from './components/loading/loading.component';
import { ErrorMessageComponent } from './components/error-message/error-message.component';

@NgModule({
  declarations: [
    AppComponent,
    CharactersComponent,
    AdventuresComponent,
    CharacterFormComponent,
    CharacterCardComponent,
    LoadingComponent,
    ErrorMessageComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi())
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
