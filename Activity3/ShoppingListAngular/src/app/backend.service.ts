import { Injectable } from '@angular/core';
import { ShoppingListItem } from './shopping-list-item';

@Injectable({
  providedIn: 'root',
})
export class BackendService {
  constructor() {}

  async shoppingList(): Promise<ShoppingListItem[]> {
    return new Promise((resolve) => {
      resolve([
        { title: 'Milk' },
        { title: 'Eggs' },
        { title: 'Stick of Butter' },
      ] as ShoppingListItem[]);
    });
  }
}
