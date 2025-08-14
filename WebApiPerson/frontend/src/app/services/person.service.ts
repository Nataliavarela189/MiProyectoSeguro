import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Person } from '../models/person.model';

@Injectable({
  providedIn: 'root'
})
export class PersonService {

  // Asegúrate que la URL coincida con el endpoint real de tu API en el back
  private apiUrl = 'http://localhost:5222/api/person';

  constructor(private http: HttpClient) { }

  // Obtener todas las personas
  getAll(): Observable<Person[]> {
    return this.http.get<Person[]>(this.apiUrl);
  }

  // Obtener persona por ID
  getById(id: number): Observable<Person> {
    return this.http.get<Person>(`${this.apiUrl}/${id}`);
  }

  // Crear nueva persona
  create(person: Person): Observable<Person> {
    return this.http.post<Person>(this.apiUrl, person);
  }

  // Actualizar persona existente
  update(id: number, person: Person): Observable<void> {
    return this.http.put<void>(`${this.apiUrl}/${id}`, person);
  }

  // Eliminar persona
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}


