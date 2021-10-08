import { Injectable } from '@angular/core';

import { NbAuthToken } from './token';
import { NbAuthTokenParceler } from './token-parceler';

export abstract class NbTokenStorage {

  abstract get(): NbAuthToken;
  abstract set(token: NbAuthToken);
  abstract clear();
  abstract setItem(item: any);
  abstract getItem();
}

/**
 * Service that uses browser localStorage as a storage.
 *
 * The token storage is provided into auth module the following way:
 * ```ts
 * { provide: NbTokenStorage, useClass: NbTokenLocalStorage },
 * ```
 *
 * If you need to change the storage behaviour or provide your own - just extend your class from basic `NbTokenStorage`
 * or `NbTokenLocalStorage` and provide in your `app.module`:
 * ```ts
 * { provide: NbTokenStorage, useClass: NbTokenCustomStorage },
 * ```
 *
 */
@Injectable()
export class NbTokenLocalStorage extends NbTokenStorage {

  protected key = 'auth_app_token';
  protected key_obj = 'auth_app_token_obj';

  constructor(private parceler: NbAuthTokenParceler) {
    super();
  }

  /**
   * Returns token from localStorage
   * @returns {NbAuthToken}
   */
  get(): NbAuthToken {
    const raw = localStorage.getItem(this.key);
    return this.parceler.unwrap(raw);
  }
  setItem(item: any) {
    localStorage.setItem(this.key_obj, item);
  }
  getItem(): any {
    const raw = localStorage.getItem(this.key_obj);
    return raw;
  }
  /**
   * Sets token to localStorage
   * @param {NbAuthToken} token
   */
  set(token: NbAuthToken) {
    const raw = this.parceler.wrap(token);
    localStorage.setItem(this.key, raw);
  }

  /**
   * Clears token from localStorage
   */
  clear() {
    localStorage.removeItem(this.key);
    localStorage.removeItem(this.key_obj);
  }
}
