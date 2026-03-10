import { Injectable, inject } from '@angular/core';
import { Router } from '@angular/router';
import { LocalStorage } from './local-storage';

type RegisteredUser = {
  name: string;
  username: string;
  email: string;
  password: string;
};

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private readonly localStorage = inject(LocalStorage);
  private readonly router = inject(Router);
  private readonly TOKEN_KEY = 'jwt_token';
  private readonly EXPIRY_KEY = 'token_expiry';
  private readonly REMEMBER_KEY = 'remember_me';
  // 2 hours session, 7 days remember me
  private readonly SESSION_EXPIRY_MS = 2 * 60 * 60 * 1000;
  private readonly REMEMBER_EXPIRY_MS = 7 * 24 * 60 * 60 * 1000;
  private lastSubstantiveRefresh = 0;

  constructor() {
    const activityHandler = () => this.refreshExpiryIfNeeded();
    window.addEventListener('mousemove', activityHandler);
    window.addEventListener('keydown', activityHandler);

    // Periodically check if session has expired, force logout if so.
    setInterval(() => {
      if (this.getToken()) {
        this.isLoggedIn(); // Evaluates expiry and auto-logs out
      }
    }, 60 * 1000); // check every minute
  }

  // Simulates a backend login and token generation
  login(identifier: string, password: string, rememberMe: boolean = false): boolean {
    const users = this.localStorage.getItem<RegisteredUser[]>('users') || [];
    // Allows sign-in using either email or username from signup.
    const foundUser = users.find(
      (u) => (u.email === identifier || u.username === identifier) && u.password === password
    );

    if (foundUser) {
      // In a real app, this token would come from a backend.
      // For demonstration, we create a simple base64 encoded string.
      const payload = { email: foundUser.email, role: 'user' }; // Assuming a default role
      const token = btoa(JSON.stringify(payload)); // Simple base64 encoding
      this.localStorage.setItem(this.TOKEN_KEY, token);
      // Set expiry based on rememberMe flag
      const expiry = Date.now() + (rememberMe ? this.REMEMBER_EXPIRY_MS : this.SESSION_EXPIRY_MS);
      this.localStorage.setItem(this.EXPIRY_KEY, expiry.toString());
      this.localStorage.setItem(this.REMEMBER_KEY, rememberMe ? 'true' : 'false');
      return true;
    }
    return false;
  }

  getToken(): string | null {
    return this.localStorage.getItem<string>(this.TOKEN_KEY);
  }

  isLoggedIn(): boolean {
    const token = this.getToken();
    if (!token) return false;
    const expiryStr = this.localStorage.getItem<string>(this.EXPIRY_KEY);
    if (!expiryStr) return false;
    const expiry = parseInt(expiryStr, 10);
    if (Date.now() > expiry) {
      // Session expired
      this.logout();
      return false;
    }
    return true;
  }

  logout(): void {
    this.localStorage.removeItem(this.TOKEN_KEY);
    this.router.navigate(['/login']);
  }

  decodeToken(): any | null {
    const token = this.getToken();
    if (token) {
      try {
        return JSON.parse(atob(token)); // Simple base64 decoding
      } catch (e) {
        console.error('Error decoding token', e);
        return null;
      }
    }
    return null;
  }

  /** Refresh expiry when user is active and not using Remember Me */
  private refreshExpiryIfNeeded(): void {
    const remember = this.localStorage.getItem(this.REMEMBER_KEY) === 'true';
    if (!remember) {
      const now = Date.now();
      // Only update local storage once every minute to prevent performance issues
      if (now - this.lastSubstantiveRefresh > 60000) {
        const newExpiry = now + this.SESSION_EXPIRY_MS;
        this.localStorage.setItem(this.EXPIRY_KEY, newExpiry.toString());
        this.lastSubstantiveRefresh = now;
      }
    }
  }
}
