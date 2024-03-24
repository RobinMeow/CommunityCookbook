import { Injectable } from '@angular/core';
import { LocalPersistorBase } from '@local-persistor';
import { RecipeLocalDto } from './util/Recipe.local-dto';
import { NewRecipe } from './util/NewRecipe';

export type RecipeId = string;

@Injectable({ providedIn: 'root' })
export class RecipeLocalPersistor extends LocalPersistorBase {
  protected override readonly storeName: string = this.StoreNames.Recipes;

  async createAsync(newRecipe: NewRecipe): Promise<RecipeId> {
    const recipeId: RecipeId = crypto.randomUUID();

    const dto: RecipeLocalDto = {
      id: recipeId,
      title: newRecipe.title,
      createdAt: new Date().toISOString(),
      synced: false,
    };

    const db = await this.getDatabaseAsync();
    await db.add(this.storeName, dto);
    return recipeId;
  }
}
