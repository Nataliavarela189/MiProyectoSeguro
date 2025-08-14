import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { tap, delay } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // Eliminamos temporalmente la URL del backend para mock
  // private apiUrl = 'http://localhost:5222/api/auth'; 

  constructor() { }

  login(username: string, password: string): Observable<any> {
    // Simulamos un login exitoso con un token falso
    return of({ token: 'fake-jwt-token' }).pipe(
      delay(1000),  // simula retardo de red
      tap(response => {
        localStorage.setItem('token', response.token);
      })
    );
  }

  logout() {
    localStorage.removeItem('token');
  }

  getToken(): string | null {
    return localStorage.getItem('token');
  }

  isLoggedIn(): boolean {
    return !!this.getToken();
  }
}

